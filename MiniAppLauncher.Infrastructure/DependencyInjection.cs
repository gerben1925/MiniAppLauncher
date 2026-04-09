using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniAppLauncher.Application.Interfaces.DataAccess;
using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Application.Services;
using MiniAppLauncher.Infrastructure.DataAccess;
using MiniAppLauncher.Infrastructure.Repositories;
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
                    .AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
