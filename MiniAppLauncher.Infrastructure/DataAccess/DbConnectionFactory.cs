using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using MiniAppLauncher.Application.Interfaces.DataAccess;

namespace MiniAppLauncher.Infrastructure.DataAccess
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection")
                                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            return new SqlConnection(connectionString);
        }
    }
}
