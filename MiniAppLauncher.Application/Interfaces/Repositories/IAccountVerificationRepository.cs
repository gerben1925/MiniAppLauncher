using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Application.Interfaces.Repositories
{
    public interface IAccountVerificationRepository
    {
        Task<int> NewAccountVerificationToken(int userId, string token);
    }
}
