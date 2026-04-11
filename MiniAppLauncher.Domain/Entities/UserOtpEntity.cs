using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Domain.Entities
{
    public  class UserOtpEntity
    {
        public int Id { get; set; }
        public Guid ReferenceKey { get; set; }
        public int UserId { get; set; }
        public string Otp { get; set; } = string.Empty;
        public int Attempts { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
