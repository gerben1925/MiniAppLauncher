using MiniAppLauncher.Domain.Entities;

namespace MiniAppLauncher.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAllUserAsync();
        Task<int> RegesterNewUser(UserEntity newUserRequest);
        Task<UserEntity?> GetUserDetailByEmail(string email);
        Task<UserEntity?> GetUserDetailByUserReference(string userReference);
        Task<bool> UpdateUserToAddPasswordAsync(UserEntity userEntity);
    }
}
