using MiniAppLauncher.Application.Features.User.Requests;
using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Application.Services;
using MiniAppLauncher.Domain.Entities;
using System.Security.Cryptography.X509Certificates;

namespace MiniAppLauncher.Application.Features.User.UseCases
{
    public class RegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailService _emailService;

       public RegisterUserUseCase(IUserRepository userRepository, IEmailTemplateService emailTemplateService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailTemplateService = emailTemplateService;
            _emailService = emailService;
        }

        public async Task<int> ExecuteTaskAsync(NewUserRequest newUserRequest)
        {
            ArgumentNullException.ThrowIfNull(newUserRequest);

            var user = MapToEntity(newUserRequest);


            var result = await _userRepository.RegesterNewUser(user);

            if (result > 0)
            {
                try
                {
                    await SendVerificationEmailAsync(user);
                }
                catch (Exception ex)
                {
                    
                }
            }
            return result;

        }

        private static UserEntity MapToEntity(NewUserRequest request)
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

        private async Task SendVerificationEmailAsync(UserEntity user)
        {
            var fullName = $"{user.FirstName} {user.LastName}".Trim();

            var (subject, body) = _emailTemplateService.BuildVerificationEmailAsync(fullName, user.Email);

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

    }
}
