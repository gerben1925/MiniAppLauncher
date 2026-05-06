namespace MiniAppLauncher.Application.Interfaces.Email
{
    public interface IEmailTemplateService
    {
        (string emailSubject, string emailBody) BuildVerificationEmailContent(string? name, string? email, string token, string verificationLink);
        (string emailSubject, string emailBody) BuildLoginOtpEmailContent(string otpCode, int expiredIn);
        (string emailSubject, string emailBody) BuildResetEmailContent(string? name, string? email, string token, string verificationLink, int expiredIn);
    }
}
