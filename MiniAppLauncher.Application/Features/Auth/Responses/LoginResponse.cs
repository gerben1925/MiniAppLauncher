namespace MiniAppLauncher.Application.Features.Auth.Responses
{
    public class LoginResponse
    {
        public string? Status { get; set; }
        public string? UserReference { get; set; }
        public int OtpExpiresInMinute { get; set; }
    }
}
