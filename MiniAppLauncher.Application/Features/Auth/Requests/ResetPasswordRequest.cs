using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Application.Features.Auth.Requests
{
    public  class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Token is required.")]
        public string? Token { get; set; }
        [Required(ErrorMessage = "New Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).+$", ErrorMessage = "Password must include uppercase, lowercase, number, and special character.")]
        public string? NewPassword { get; set; }
    }
}
