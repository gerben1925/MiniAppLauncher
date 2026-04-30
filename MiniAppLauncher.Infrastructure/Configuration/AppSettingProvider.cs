using Microsoft.Extensions.Configuration;
using MiniAppLauncher.Application.Interfaces.Configuration;

namespace MiniAppLauncher.Infrastructure.Configuration
{
    public class AppSettingProvider : IAppSettingProvider
    {
        private readonly IConfiguration _configuration;

        public AppSettingProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetString(string key)
        {
            return _configuration.GetValue<string>(key) ?? string.Empty;
        }

        public int GetInt(string key)
        {
            return _configuration.GetValue<int>(key);
        }

        public bool GetBool(string key)
        {
            return _configuration.GetValue<bool>(key);
        }
        

    }
}
