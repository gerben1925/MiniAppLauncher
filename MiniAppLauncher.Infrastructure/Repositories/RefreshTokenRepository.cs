using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Domain.Entities;
using MiniAppLauncher.Infrastructure.DataAccess;

namespace MiniAppLauncher.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DapperExecutor _dapperExecutor;
        public RefreshTokenRepository(DapperExecutor dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public async Task<int> SaveTokenAsync(RefreshTokenEntity refreshTokenEntity)
        {
            const string sql = @"
                              
                            INSERT INTO RefreshTokens 
                            (UserId, Token, CreatedAt, ExpiredAt, RevokedAt, IsRevoked)
                            VALUES
                            (@UserId, @Token, @CreatedAt, @ExpiredAt, @RevokedAt, @IsRevoked );
                            
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                            ";


            var result = await _dapperExecutor.ExecuteScalarAsync<int>(sql, refreshTokenEntity);

            return result;
        }

    }
}
