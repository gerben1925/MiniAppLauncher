using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Domain.Entities;
using MiniAppLauncher.Infrastructure.DataAccess;

namespace MiniAppLauncher.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
    
        private readonly DapperExecutor _dapperExecutor;
        
        public UserRepository(DapperExecutor  dapperExecutor) 
        { 
            _dapperExecutor = dapperExecutor;
        }

       
        public async Task<IEnumerable<UserEntity>> GetAllUserAsync()
        {
            var sqlQuery = @"
                    SELECT 
                        UserID,
                        UserReference,
                        LastName,
                        FirstName,
                        PasswordHash,
                        PasswordSalt,
                        Email,
                        RoleID,
                        CreatedAt,
                        IsActive,
                        Notes
                    FROM [dbo].[Users]
                    ORDER BY UserReference";

            var users = await _dapperExecutor.QueryAsync<UserEntity>(sqlQuery);
            return users ?? Enumerable.Empty<UserEntity>();
        }

        public async Task<int> RegesterNewUser(UserEntity userEntity)
        {
            const string sql = @"
                              
                            INSERT INTO Users 
                            (LastName, FirstName, Email, RoleID, Notes)
                            VALUES
                            (@LastName,@FirstName, @Email, @RoleID, @Notes );
                            
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                            ";


            var result = await _dapperExecutor.ExecuteScalarAsync<int>(sql, userEntity);

            return result;  
        }

    }
}
