using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Domain.Entities
{
    public class PasswordResetEntity
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string Token { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
