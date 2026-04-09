using Microsoft.AspNetCore.Connections;
using MiniAppLauncher.Infrastructure.DataAccess;

namespace MiniAppLauncher.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceCollections(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration.GetConnectionString("DefaultConnection")
            //   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


            //services.AddScoped<IDBConnectionFactory, DBConnectionFactory>(_ =>
            //                    new DBConnectionFactory(connectionString));

            

            return services;
        }
    }
}
