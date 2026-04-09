using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Domain.Entities;
using MiniAppLauncher.Infrastructure.DataAccess;

namespace MiniAppLauncher.Infrastructure.Repositories
{
    public  class AccountVerificationRepository : IAccountVerificationRepository
    {
        private readonly DapperExecutor _dapperExecutor;
        public AccountVerificationRepository(DapperExecutor dapperExecutor) 
        {
            _dapperExecutor = dapperExecutor;
        }

        public async Task<int> NewAccountVerificationToken(int userId, string token)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid userId");

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be empty");

            var entity = new AccountVerificationEntity
            {
                UserID = userId,
                Token = token,
                CreatedAt = DateTime.UtcNow
            };


            const string sql = @"
                              
                            INSERT INTO AccountVerificationTokens 
                            (UserID, Token)
                            VALUES
                            (@UserID,@Token);
                            
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                            ";


            var result = await _dapperExecutor.ExecuteScalarAsync<int>(sql, entity);
            return result;
        }
    }
}
