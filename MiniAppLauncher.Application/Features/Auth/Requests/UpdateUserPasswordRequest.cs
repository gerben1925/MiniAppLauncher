using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Application.Features.Auth.Requests
{
    public class UpdateUserPasswordRequest
    {
        public int UserId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public byte[]? PasswordSalt { get; set; }
    }
}
