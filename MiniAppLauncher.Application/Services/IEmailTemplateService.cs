namespace MiniAppLauncher.Application.Services
{
    public interface IEmailTemplateService
    {
        (string emailSubject, string emailBody) BuildVerificationEmailAsync( string? name, string? email);
    }
}
