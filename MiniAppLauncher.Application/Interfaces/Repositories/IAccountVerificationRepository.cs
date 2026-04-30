using MiniAppLauncher.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Application.Interfaces.Repositories
{
    public interface IAccountVerificationRepository
    {
        Task<int> NewAccountVerificationToken(AccountVerificationEntity accountVerificationEntity);
        Task<AccountVerificationEntity?> GetAccountVerificationByToken(string token);
        Task<bool> UpdateUserToAddPasswordAsync(string token, int userId);
    }
}
