
using MiniAppLauncher.Application.Common;
using MiniAppLauncher.Application.Features.Auth.Requests;
using MiniAppLauncher.Application.Features.Auth.Responses;
using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Application.Interfaces.Security;
using MiniAppLauncher.Domain.Entities;

namespace MiniAppLauncher.Application.Features.Auth.UseCases
{
    public class VerifyLoginOtpUseCase 
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserOtpRepository _userOtpRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public VerifyLoginOtpUseCase(IUserRepository userRepository, IUserOtpRepository userOtpRepository, IJwtTokenService jwtTokenService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _userOtpRepository = userOtpRepository;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<OperationResult<VerifyLoginOtpResponse>> ExecuteAsync(VerifyLoginOtpRequest request)
        {
            var userReference = request.UserReference ?? string.Empty;

            var user = await _userRepository.GetUserDetailByUserReference(userReference);
            if (user is null)
                return OperationResult<VerifyLoginOtpResponse>.NotFound("User not found.");

            var otpResult = await ValidateOtpAsync(userReference, request.Otp);
            if (!otpResult.IsSuccess)
                return OperationResult<VerifyLoginOtpResponse>.Unauthorized(otpResult.ErrorMessage!);

            var tokenResult = await GenerateAndSaveTokensAsync(user);
            if (!tokenResult.IsSuccess)
                return OperationResult<VerifyLoginOtpResponse>.InternalServerError("Failed to save refresh token.");

            return OperationResult<VerifyLoginOtpResponse>.Success(tokenResult.Data!);
        }

        private async Task<OperationResult<UserOtpEntity>> ValidateOtpAsync(string userReference, int otp)
        {
            var userOtp = await _userOtpRepository.VerifyOtpByUserReference(userReference, otp);

            if (userOtp is null || userOtp.ExpiresAt < DateTime.UtcNow)
                return OperationResult<UserOtpEntity>.Unauthorized("Invalid or expired OTP.");

            return OperationResult<UserOtpEntity>.Success(userOtp,200);
        }

        private async Task<OperationResult<VerifyLoginOtpResponse>> GenerateAndSaveTokensAsync(UserEntity user)
        {
            var accessToken = _jwtTokenService.GenerateAccessToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshTokenEntity
            {
                UserId = user.UserID,
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = _jwtTokenService.GetRefreshTokenExpiration(),
                IsRevoked = false
            };

            var savedId = await _refreshTokenRepository.SaveTokenAsync(refreshTokenEntity);
            if (savedId <= 0)
                return OperationResult<VerifyLoginOtpResponse>.InternalServerError("Failed to save refresh token.");

            return OperationResult<VerifyLoginOtpResponse>.Success(new VerifyLoginOtpResponse
            {
                UserReference = user.UserReference.ToString(),
                AccessToken = accessToken,
            });
        }

    }
}
