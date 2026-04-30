using Microsoft.Extensions.Configuration;
using MiniAppLauncher.Application.Interfaces.Configuration;
using MiniAppLauncher.Application.Interfaces.Security;
using System.Text;

namespace MiniAppLauncher.Infrastructure.Security
{
    public class OtpGenerator : IOtpGenerator
    {
        private readonly IAppSettingProvider _appSettingProvider;
        public OtpGenerator(IAppSettingProvider appSettingProvider)
        {
            _appSettingProvider = appSettingProvider;
        }

        public (string otp, DateTime createdAt, DateTime expiresAt, int expiresInMinutes) GenerateOtp()
        {
            int otpLength = _appSettingProvider.GetInt("OtpSettings:Length");
            int expiryMinutes = _appSettingProvider.GetInt("OtpSettings:ExpirationMinutes");

            var now = DateTime.UtcNow;

            var otp = GenerateNumericOtp(otpLength);
            var expiresAt = now.AddMinutes(expiryMinutes);

            return (otp, now, expiresAt, expiryMinutes);
        }

        private static string GenerateNumericOtp(int length)
        {
            var random = new Random();
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(random.Next(0, 10));
            }

            return sb.ToString();
        }


    }
}
