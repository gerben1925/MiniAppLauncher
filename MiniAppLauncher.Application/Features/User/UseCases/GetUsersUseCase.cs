using MiniAppLauncher.Application.Features.User.Responses;
using MiniAppLauncher.Application.Interfaces.Repositories;
using System.Globalization;

namespace MiniAppLauncher.Application.Features.User.UseCases
{
    public class GetUsersUseCase
    {
        private readonly IUserRepository _userRepository;
        public GetUsersUseCase(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponse>> ExecuteTaskAsync() 
        {
            var users = await _userRepository.GetAllUserAsync();

            var result = users.Select(u => new UserResponse 
            { 
                UserID = u.UserID,
                UserReference = u.UserReference,
                LastName = u.LastName,
                FirstName = u.FirstName,
                PasswordHash = u.PasswordHash,
                PasswordSalt = u.PasswordSalt,
                Email = u.Email,
                RoleID = u.RoleID,
                CreatedAt  = u.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                IsActive = u.IsActive,
                Notes = u.Notes,
            });
            
            return result;
        }
    }
}
