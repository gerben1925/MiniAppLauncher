using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniAppLauncher.Application.Interfaces.Common;
using MiniAppLauncher.Application.Interfaces.Configuration;
using MiniAppLauncher.Application.Interfaces.DataAccess;
using MiniAppLauncher.Application.Interfaces.Email;
using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Application.Interfaces.Security;
using MiniAppLauncher.Infrastructure.Common;
using MiniAppLauncher.Infrastructure.Configuration;
using MiniAppLauncher.Infrastructure.DataAccess;
using MiniAppLauncher.Infrastructure.Repositories;
using MiniAppLauncher.Infrastructure.Security;
using MiniAppLauncher.Infrastructure.Services;


namespace MiniAppLauncher.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DapperExecutor>()
                    .AddScoped<IDbConnectionFactory, DbConnectionFactory>()
                    .AddScoped<IUserRepository, UserRepository>()
                    .AddScoped<IAccountVerificationRepository, AccountVerificationRepository>()
                    .AddScoped<IEmailTemplateService, EmailTemplateService>()
                    .AddScoped<IEmailService, EmailService>()
                    .AddScoped<IPasswordHasher, PasswordHasher>()
                    .AddScoped<IOtpGenerator, OtpGenerator>()
                    .AddScoped<IUserOtpRepository , UserOtpRepository>()
                    .AddScoped<IJwtTokenService, JwtTokenService>()
                    .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
                    .AddScoped<IAccountVerificationRepository, AccountVerificationRepository>()
                    .AddScoped<IAppSettingProvider, AppSettingProvider>()
                    .AddScoped<IStringGenerator, StringGenerator>()
                    .AddScoped<IAccountVerificationRepository , AccountVerificationRepository>()
                    .AddScoped<IPasswordResetRepository, PasswordResetRepository>();

            return services;
        }
    }
}
