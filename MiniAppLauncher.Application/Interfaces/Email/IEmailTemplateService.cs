namespace MiniAppLauncher.Application.Interfaces.Email
{
    public interface IEmailTemplateService
    {
        (string emailSubject, string emailBody) BuildVerificationEmailContent(string? name, string? email, string token, string verificationLink);
        (string emailSubject, string emailBody) BuildLoginOtpEmailContent(string otpCode, int expiredIn);
    }
}
