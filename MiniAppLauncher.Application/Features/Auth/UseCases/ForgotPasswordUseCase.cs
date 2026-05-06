using MiniAppLauncher.Application.Common;
using MiniAppLauncher.Application.Features.Auth.Requests;
using MiniAppLauncher.Application.Features.Auth.Responses;
using MiniAppLauncher.Application.Features.User.Requests;
using MiniAppLauncher.Application.Interfaces.Common;
using MiniAppLauncher.Application.Interfaces.Configuration;
using MiniAppLauncher.Application.Interfaces.Email;
using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Domain.Entities;

namespace MiniAppLauncher.Application.Features.Auth.UseCases
{
    public class ForgotPasswordUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountVerificationRepository _accountVerificationRepository;
        private readonly IPasswordResetRepository _passwordResetRepository;
        private readonly IAppSettingProvider _appSettingProvider;
        private readonly IStringGenerator _stringGenerator;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;

        public ForgotPasswordUseCase(IUserRepository userRepository, IAccountVerificationRepository accountVerificationRepository, IPasswordResetRepository passwordResetRepository, IAppSettingProvider appSettingProvider, IStringGenerator stringGenerator, IEmailService emailService, IEmailTemplateService emailTemplateService)
        {
            _userRepository = userRepository;
            _accountVerificationRepository = accountVerificationRepository;
            _passwordResetRepository = passwordResetRepository;
            _appSettingProvider = appSettingProvider;
            _stringGenerator = stringGenerator;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
        }

 

        public async Task<OperationResult<ResetPasswordResponse>> ExecuteAsync(ForgotPasswordRequest request)
        {
            var userResult = await ValidateUserEmailAsync(request.Email ?? string.Empty);
            if (!userResult.IsSuccess)
                return OperationResult<ResetPasswordResponse>.Unauthorized(userResult.ErrorMessage!);

            var user = userResult.Data;
            var token = GenerateResetPasswordToken();
            var expirationMinutes = GetResetPasswordExpirationMinutes();

            var savedId = await SaveResetPasswordAsync(user.UserID, token, expirationMinutes);
            if (savedId == 0)
                return OperationResult<ResetPasswordResponse>.InternalServerError("Failed to save reset password.");

            var resetLink = BuildResetLink(token);
            await SendResetEmailAsync(user, token, resetLink, expirationMinutes);

            return OperationResult<ResetPasswordResponse>.SuccessMessageOnly("Password reset email sent successfully.");
        }

        private static PasswordResetEntity MapToNewPasswordResetEntity(int userId, string token, int expirationMinutes)
        {

            return new PasswordResetEntity
            {
                UserID = userId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
                IsUsed = false
            };
        }

        private async Task<OperationResult<UserEntity>> ValidateUserEmailAsync(string email)
        {
            var userDetails = await _userRepository.GetUserDetailByEmail(email);

            if (userDetails is null)
                return OperationResult<UserEntity>.Unauthorized("Please provide a valid email!");

            return OperationResult<UserEntity>.Success(userDetails, 200);
        }

        private string GenerateResetPasswordToken()
        {
            var tokenLength = _appSettingProvider.GetInt("ForgotPasswordSettings:TokenLength");
            return _stringGenerator.GenerateRandomString(tokenLength);
        }

        private int GetResetPasswordExpirationMinutes()
        {
            var tokenLength = _appSettingProvider.GetInt("ForgotPasswordSettings:ExpirationMinutes");
            return tokenLength;
        }

        private string BuildResetLink(string token)
        {
            var baseUrl = _appSettingProvider.GetString("FrontEnd:BaseUrl");
            return $"{baseUrl.TrimEnd('/')}/reset-password?token={token}";
        }

        private async Task SendResetEmailAsync(UserEntity user, string token, string verificationLink, int expiration)
        {
            var fullName = $"{user.FirstName} {user.LastName}".Trim();
            var emailContent = _emailTemplateService.BuildResetEmailContent(fullName, user.Email, token, verificationLink, expiration);
            await _emailService.SendEmailAsync(user.Email, emailContent.emailSubject, emailContent.emailBody);
        }

        private async Task<int> SaveResetPasswordAsync(int userId, string token, int expirationMinutes)
        {
            var entity = MapToNewPasswordResetEntity(userId, token, expirationMinutes);
            return await _passwordResetRepository.NewPasswordResetAsync(entity);
        }

    }
}
