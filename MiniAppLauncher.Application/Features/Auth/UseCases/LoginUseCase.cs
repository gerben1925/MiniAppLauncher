using MiniAppLauncher.Application.Common;
using MiniAppLauncher.Application.Features.Auth.Requests;
using MiniAppLauncher.Application.Features.Auth.Responses;
using MiniAppLauncher.Application.Interfaces.Email;
using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Application.Interfaces.Security;
using MiniAppLauncher.Domain.Entities;

namespace MiniAppLauncher.Application.Features.Auth.UseCases
{
    public  class LoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IOtpGenerator _otpGenerator;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailService _emailService;
        private readonly IUserOtpRepository _userOtpRepository;
        public LoginUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher, IOtpGenerator otpGenerator, IEmailTemplateService emailTemplateService, IEmailService emailService, IUserOtpRepository userOtpRepository) 
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _otpGenerator = otpGenerator;
            _emailTemplateService = emailTemplateService;
            _emailService = emailService;
            _userOtpRepository = userOtpRepository;
        }

        
        public async Task<OperationResult<LoginResponse>> ExecuteAsync(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetUserDetailByEmail(loginRequest.Email ?? string.Empty);

            if (user is null)
                return OperationResult<LoginResponse>.BadRequest("Invalid email.");

            if (!user.IsActive)
                return OperationResult<LoginResponse>.BadRequest("User account is inactive.");

            if (!IsPasswordValid(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
                return OperationResult<LoginResponse>.BadRequest("Invalid login credentials.");

            var response = await GenerateAndDispatchOtpAsync(user.UserID, user.UserReference, user.Email);

            return OperationResult<LoginResponse>.Success(response, 200);
        }

        private bool IsPasswordValid(string? password, string? passwordHash, byte[] passwordSalt) 
        {
           return _passwordHasher.VerifyPassword(
                   password ?? string.Empty,
                   passwordHash ?? string.Empty,
                   passwordSalt);
        }
           

        private async Task<LoginResponse> GenerateAndDispatchOtpAsync(int userId, Guid userReference, string userEmail)
        {
            var (otp, createdAt, expiresAt, expiresInMinutes) = _otpGenerator.GenerateOtp();

            await TrySaveAndEmailOtpAsync(userId, otp, createdAt, expiresAt, expiresInMinutes, userEmail);

            return new LoginResponse
            {
                Status = "OTP sent to email.",
                UserReference = userReference.ToString(),
                OtpExpiresInMinute = expiresInMinutes,
            };
        }

        private async Task TrySaveAndEmailOtpAsync( int userId,string otp,DateTime createdAt,DateTime expiresAt,int expiresInMinutes, string userEmail)
        {
            var otpEntity = new UserOtpEntity
            {
                UserId = userId,
                Otp = otp,
                Attempts = 1,
                CreatedAt = createdAt,
                ExpiresAt = expiresAt,
            };

            var savedId = await _userOtpRepository.SavedNewUserOtp(otpEntity);

            if (savedId <= 0) return;

            var (subject, body) = _emailTemplateService.BuildLoginOtpEmailContent(otp, expiresInMinutes);
            await _emailService.SendEmailAsync(userEmail, subject, body);
        }



    }
}
