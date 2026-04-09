using Microsoft.Extensions.DependencyInjection;
using MiniAppLauncher.Application.Features.User.UseCases;


namespace MiniAppLauncher.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationCollection(this IServiceCollection services) 
        {
            services.AddScoped<GetUsersUseCase>()
                    .AddScoped<RegisterUserUseCase>();
                    

            return services;
        }
    }
}
