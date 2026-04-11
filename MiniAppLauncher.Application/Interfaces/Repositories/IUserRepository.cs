using MiniAppLauncher.Application.Features.User.Requests;
using MiniAppLauncher.Application.Features.User.Responses;
using MiniAppLauncher.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAllUserAsync();
        Task<int> RegesterNewUser(UserEntity newUserRequest);
        Task<UserEntity?> GetUserDetailByEmail(string email);
        Task<UserEntity?> GetUserDetailByUserReference(string userReference);
    }
}
