using MiniAppLauncher.Application.Features.User.Requests;
using MiniAppLauncher.Application.Interfaces.Common;
using MiniAppLauncher.Application.Interfaces.Configuration;
using MiniAppLauncher.Application.Interfaces.Email;
using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Domain.Entities;

namespace MiniAppLauncher.Application.Features.User.UseCases
{
    public class RegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailService _emailService;
        private readonly IAccountVerificationRepository _accountVerificationRepository;
        private readonly IStringGenerator _stringGenerator;
        private readonly IAppSettingProvider _appSettingProvider;

        public RegisterUserUseCase(IUserRepository userRepository, IEmailTemplateService emailTemplateService, IEmailService emailService, IAccountVerificationRepository accountVerificationRepository, IStringGenerator stringGenerator, IAppSettingProvider appSettingProvider)
        {
            _userRepository = userRepository;
            _emailTemplateService = emailTemplateService;
            _emailService = emailService;
            _accountVerificationRepository = accountVerificationRepository;
            _stringGenerator = stringGenerator;
            _appSettingProvider = appSettingProvider;
        }

        public async Task<int> ExecuteTaskAsync(NewUserRequest newUserRequest)
        {
            ArgumentNullException.ThrowIfNull(newUserRequest);

            var user = MapToUserEntity(newUserRequest);
            int userId = await _userRepository.RegesterNewUser(user);

            if (userId <= 0) return 0;

            var token = GenerateVerificationToken();
            var verificationLink = BuildVerificationLink(token);
            await SendVerificationEmailAsync(user, token, verificationLink);
            return await SaveVerificationTokenAsync(userId, token);
        }

        private static UserEntity MapToUserEntity(NewUserRequest request)
        {
            return new UserEntity
            {
                LastName = request.LastName,
                FirstName = request.FirstName,
                Email = request.Email,
                RoleID = request.RoleID,
                Notes = request.Notes
            };
        }

        private static AccountVerificationEntity MapToNewAccountVerificationEntity(int userId, string token)
        {
            return new AccountVerificationEntity
            {
                UserID = userId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                IsUsed = false
            };
        }

        private string GenerateVerificationToken()
        {
            var tokenLength = _appSettingProvider.GetInt("EmailVerificationAccountSettings:TokenLength");
            return _stringGenerator.GenerateRandomString(tokenLength);
        }

        private string BuildVerificationLink(string token)
        {
            var baseUrl = _appSettingProvider.GetString("FrontEnd:BaseUrl");
            return $"{baseUrl.TrimEnd('/')}/verify-account?token={token}";
        }

        private async Task SendVerificationEmailAsync(UserEntity user, string token, string verificationLink)
        {
            var fullName = $"{user.FirstName} {user.LastName}".Trim();
            var emailContent = _emailTemplateService.BuildVerificationEmailContent(fullName, user.Email, token, verificationLink);
            await _emailService.SendEmailAsync(user.Email, emailContent.emailSubject, emailContent.emailBody);
        }

        private async Task<int> SaveVerificationTokenAsync(int userId, string token)
        {
            var entity = MapToNewAccountVerificationEntity(userId, token);
            return await _accountVerificationRepository.NewAccountVerificationToken(entity);
        }



    }
}
