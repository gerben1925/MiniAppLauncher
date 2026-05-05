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

    }
}
