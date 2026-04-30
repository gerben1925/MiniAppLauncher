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

        public async Task<int> NewAccountVerificationToken(AccountVerificationEntity accountVerificationEntity)
        {
            const string sql = @"
                              
                            INSERT INTO AccountVerificationTokens 
                            (UserID, Token)
                            VALUES
                            (@UserID,@Token);
                            
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                            ";


            var result = await _dapperExecutor.ExecuteScalarAsync<int>(sql, accountVerificationEntity);
            return result;
        }

        public async Task<AccountVerificationEntity?> GetAccountVerificationByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            const string sqlQuery = @"
                    SELECT TOP 1
                        TokenId,
                        UserID,
                        Token,
                        CreatedAt,
                        IsUsed
                    FROM AccountVerificationTokens
                    Where Token = @Token ";

            var result = await _dapperExecutor.QuerySingleAsync<AccountVerificationEntity>(sqlQuery, new { Token = token });
            return result;
        }

        public async Task<bool> UpdateUserToAddPasswordAsync(string token, int userId)
        {
            const string sql = @"
                            UPDATE AccountVerificationTokens 
                                SET IsUsed = 1
                                WHERE UserId = @UserId AND Token = @Token
                            ";


            var result = await _dapperExecutor.ExecuteAsync(sql, new {Token = token, UserId  = userId});
       
            return result > 0;
        }


    }
}
