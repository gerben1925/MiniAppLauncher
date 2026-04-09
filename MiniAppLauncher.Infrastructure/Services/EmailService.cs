using Microsoft.Extensions.Configuration;
using MiniAppLauncher.Application.Services;
using MiniAppLauncher.Infrastructure.Helper;
using System.Net;
using System.Net.Mail;

namespace MiniAppLauncher.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
   
        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }  
        
        public async Task SendEmailAsync(string to, string subject, string body)
        {
           
                
                using var smtp = new SmtpClient
                {
                    Host = _config["SmtpCredentials:Host"] ?? string.Empty,
                    Port = _config.GetValue<int>("SmtpCredentials:Port"),
                    EnableSsl = true,
                    Credentials = new NetworkCredential(
                        _config["SmtpCredentials:NetworkCredentialUsername"],
                        _config["SmtpCredentials:NetworkCredentialPass"]
                    )
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_config["SmtpCredentials:FROMEmail"] ?? string.Empty),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(to);

                await smtp.SendMailAsync(message);
        }


    }
}
