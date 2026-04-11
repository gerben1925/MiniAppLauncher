using Dapper;
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
         const string sqlQuery = @"
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

        public async Task<UserEntity?> GetUserDetailByEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                return null;

            const string sqlQuery = @"
                    SELECT TOP 1
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
                    Where Email = @Email ";

            var result = await _dapperExecutor.QuerySingleAsync<UserEntity>(sqlQuery, new { Email = email });
            return result;
        }

        public async Task<UserEntity?> GetUserDetailByUserReference(string userReference)
        {
            if (string.IsNullOrWhiteSpace(userReference))
                return null;

            const string sqlQuery = @"
                            SELECT TOP 1
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
                            WHERE UserReference = @UserReference";

            return await _dapperExecutor.QuerySingleAsync<UserEntity>(sqlQuery, new { UserReference = userReference });
        }

    }
}
