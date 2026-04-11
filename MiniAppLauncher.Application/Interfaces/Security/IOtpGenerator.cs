namespace MiniAppLauncher.Application.Interfaces.Security
{
    public interface IOtpGenerator
    {
        (string otp, DateTime createdAt, DateTime expiresAt, int expiresInMinutes) GenerateOtp();
    }
}
