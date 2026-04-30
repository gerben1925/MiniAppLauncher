using MiniAppLauncher.Application.Common;
using MiniAppLauncher.Application.Features.Auth.Requests;
using MiniAppLauncher.Application.Features.Auth.Responses;
using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Application.Interfaces.Security;
using MiniAppLauncher.Domain.Entities;

namespace MiniAppLauncher.Application.Features.Auth.UseCases
{
    public class ActivateUserAccountUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountVerificationRepository _accountVerificationRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ActivateUserAccountUseCase(IUserRepository userRepository, IAccountVerificationRepository accountVerificationRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _accountVerificationRepository = accountVerificationRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<OperationResult<AccountVerificationEntity>> ExecuteAsync(VerifyAccountRequest request)
        {
            var validationResult = await ValidateRequestAsync(request);
            if (!validationResult.IsSuccess)
                return validationResult;

  
            if (validationResult.Data == null)
                return OperationResult<AccountVerificationEntity>.InternalServerError("Verification data is missing.");


            int userId = validationResult.Data.UserID;


            var isPasswordSet = await SetUserPasswordAsync(request.Password!, userId);
            if (!isPasswordSet)
                return OperationResult<AccountVerificationEntity>.InternalServerError("Failed to set user password.");


            var isRevoked = await RevokeVerificationTokenAsync(request.Token, userId);
            if (!isRevoked)
                return OperationResult<AccountVerificationEntity>.InternalServerError("Failed to revoke verification token.");

            return OperationResult<AccountVerificationEntity>.SuccessMessageOnly("Account successfully activated.");
        }


        private async Task<OperationResult<AccountVerificationEntity>> ValidateRequestAsync(VerifyAccountRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Token))
                return OperationResult<AccountVerificationEntity>.BadRequest("Please provide a token.");

            if (string.IsNullOrWhiteSpace(request.Password))
                return OperationResult<AccountVerificationEntity>.BadRequest("Please provide a password.");

            var verification = await _accountVerificationRepository.GetAccountVerificationByToken(request.Token);

            if (verification == null)
                return OperationResult<AccountVerificationEntity>.Unauthorized("Invalid token: token not found.");

            if (verification.IsUsed)
                return OperationResult<AccountVerificationEntity>.Unauthorized("Invalid token: it has already been used.");

            return OperationResult<AccountVerificationEntity>.Success(verification);
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


        private async Task<bool> RevokeVerificationTokenAsync(string token, int userId)
        {
            return await _accountVerificationRepository.UpdateUserToAddPasswordAsync(token, userId);
        }

    }
}
