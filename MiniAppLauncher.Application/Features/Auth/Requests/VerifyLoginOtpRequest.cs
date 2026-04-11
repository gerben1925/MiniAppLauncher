using System.ComponentModel.DataAnnotations;

namespace MiniAppLauncher.Application.Features.Auth.Requests
{
    public  class VerifyLoginOtpRequest
    {
        [Required(ErrorMessage = "Token is required.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "OTP must be numeric.")]
        public int Otp { get; set; }
        [Required(ErrorMessage = "User Reference is required.")]
        public string? UserReference { get; set; }
    }
}
