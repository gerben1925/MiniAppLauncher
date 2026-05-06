using MiniAppLauncher.Application.Common;
using MiniAppLauncher.Application.Features.Auth.Requests;
using MiniAppLauncher.Application.Features.Auth.Responses;
using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Application.Interfaces.Security;
using MiniAppLauncher.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Application.Features.Auth.UseCases
{
    public class ResetPasswordUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordResetRepository _passwordResetRepository;

        public ResetPasswordUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher, IPasswordResetRepository passwordResetRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _passwordResetRepository = passwordResetRepository;
        }

        public async Task<OperationResult<ResetPasswordResponse>> ExecuteAsync(ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NewPassword))
                return OperationResult<ResetPasswordResponse>.BadRequest("Token and new password are required.");

            var resetPasswordResult = await ValidateResetPasswordAsync(request.Token);
            if (!resetPasswordResult.IsSuccess)
                return OperationResult<ResetPasswordResponse>.BadRequest(resetPasswordResult.ErrorMessage!);

            if (resetPasswordResult.Data is not { } resetPassword)
                return OperationResult<ResetPasswordResponse>.InternalServerError("Failed to retrieve reset password details.");

            bool isTokenMarkedUsed = await MarkTokenAsUsedAsync(request.Token, resetPassword.UserID);
            if (!isTokenMarkedUsed)
                return OperationResult<ResetPasswordResponse>.InternalServerError("Failed to invalidate reset token.");

            bool isPasswordUpdated = await SetUserPasswordAsync(request.NewPassword, resetPassword.UserID);
            if (!isPasswordUpdated)
                return OperationResult<ResetPasswordResponse>.InternalServerError("Failed to update password.");

            return OperationResult<ResetPasswordResponse>.SuccessMessageOnly("Password reset successful.");
        }

        private async Task<OperationResult<PasswordResetEntity>> ValidateResetPasswordAsync(string token)
        {
            var resetPasswordDetails = await _passwordResetRepository.GetPasswordResetByToken(token);
            if (resetPasswordDetails is null || resetPasswordDetails.ExpiredAt < DateTime.UtcNow || resetPasswordDetails.IsUsed == true)
                return OperationResult<PasswordResetEntity>.BadRequest("Token is invalid or has expired.");

            return OperationResult<PasswordResetEntity>.Success(resetPasswordDetails, 200);
        }

        private async Task<bool> MarkTokenAsUsedAsync(string token, int userId)
        {
            return await _passwordResetRepository.UpdatePasswordResetAsync(token, userId);
        }

        private async Task<bool> SetUserPasswordAsync(string password, int userId)
        {
            var (hashedPassword, salt) = _passwordHasher.HashPassword(password);
            var userEntity = new UserEntity
            {
                UserID = userId,
                PasswordHash = hashedPassword,
                PasswordSalt = salt
            };
            return await _userRepository.UpdateUserToAddPasswordAsync(userEntity);
        }
    }
}
