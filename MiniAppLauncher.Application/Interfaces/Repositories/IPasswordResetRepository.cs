using MiniAppLauncher.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Application.Interfaces.Repositories
{
    public interface IPasswordResetRepository
    {
        Task<int> NewPasswordResetAsync(PasswordResetEntity passwordResetEntity);
    }
}
