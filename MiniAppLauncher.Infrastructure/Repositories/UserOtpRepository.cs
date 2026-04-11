using MiniAppLauncher.Application.Interfaces.Repositories;
using MiniAppLauncher.Domain.Entities;
using MiniAppLauncher.Infrastructure.DataAccess;

namespace MiniAppLauncher.Infrastructure.Repositories
{
    public class UserOtpRepository : IUserOtpRepository
    {
        private readonly DapperExecutor _dapperExecutor;

        public UserOtpRepository(DapperExecutor dapperExecutor)
        {
            _dapperExecutor = dapperExecutor;
        }

        public async Task<int> SavedNewUserOtp(UserOtpEntity userOtpEntity)
        {
            const string sql = @"
                              
                            INSERT INTO UserOtps 
                            (UserId, Otp, Attempts, ExpiresAt, CreatedAt)
                            VALUES
                            (@UserId ,@Otp , @Attempts, @ExpiresAt, @CreatedAt);
                            
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                            ";


            var result = await _dapperExecutor.ExecuteScalarAsync<int>(sql, userOtpEntity);

            return result;
        }


        public async Task<UserOtpEntity?> VerifyOtpByUserReference(string userReference, int otp)
        {
            if (string.IsNullOrWhiteSpace(userReference) || otp == 0)
                return null;

            const string sqlQuery = @"
                              SELECT TOP 1
	                            OTP.Id,
	                            OTP.ReferenceKey,
	                            OTP.UserId,
	                            OTP.Otp,
	                            OTP.Attempts,
	                            OTP.ExpiresAt,
	                            OTP.IsUsed,
	                            OTP.CreatedAt
                                FROM [dbo].[UserOtps] AS OTP
	                            LEFT JOIN [dbo].[Users] AS U
	                            ON
	                            OTP.UserId = U.UserId
                            WHERE [Otp] = @Otp AND UserReference = @UserReference";

            return await _dapperExecutor.QuerySingleAsync<UserOtpEntity>(sqlQuery, new { UserReference = userReference, Otp = otp });
        }

    }
}
