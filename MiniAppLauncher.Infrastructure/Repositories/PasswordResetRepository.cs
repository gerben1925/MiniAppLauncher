using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Domain.Entities;
using MiniAppLauncher.Infrastructure.DataAccess;

namespace MiniAppLauncher.Infrastructure.Repositories
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly DapperExecutor _dapperExecutor;

        public PasswordResetRepository(DapperExecutor dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }
        public async Task<int> NewPasswordResetAsync(PasswordResetEntity passwordResetEntity)
        {
            const string sql = @"
                              
                            INSERT INTO PasswordReset 
                            (UserId, Token, CreatedAt, ExpiredAt, IsUsed)
                            VALUES
                            (@UserID, @Token, @CreatedAt, @ExpiredAt, @IsUsed);
                            
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                            ";


            var result = await _dapperExecutor.ExecuteScalarAsync<int>(sql, passwordResetEntity);
            return result;
        }

        public async Task<PasswordResetEntity?> GetPasswordResetByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            const string sqlQuery = @"
                    SELECT TOP 1
                        [Id],
                        [UserId],
                        [Token],
                        [CreatedAt],
	                    [ExpiredAt],
                        IsUsed
                    FROM [dbo].[PasswordReset]
                    Where Token = @Token ";

            var result = await _dapperExecutor.QuerySingleAsync<PasswordResetEntity>(sqlQuery, new { Token = token });
            return result;
        }

        public async Task<bool> UpdatePasswordResetAsync(string token, int userId)
        {
            const string sql = @"
                            UPDATE [dbo].[PasswordReset]
                                SET IsUsed = 1
                                WHERE UserId = @UserId AND Token = @Token
                            ";


            var result = await _dapperExecutor.ExecuteAsync(sql, new { Token = token, UserId = userId });

            return result > 0;
        }


    }
}
