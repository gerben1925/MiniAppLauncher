using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Domain.Entities
{
    public  class AccountVerificationEntity
    {
        public int TokenId { get; set; }
        public int UserID { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
