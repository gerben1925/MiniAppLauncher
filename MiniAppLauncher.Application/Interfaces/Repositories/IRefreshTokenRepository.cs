using MiniAppLauncher.Domain.Entities;

namespace MiniAppLauncher.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<int> SaveTokenAsync(RefreshTokenEntity refreshTokenEntity);
    }
}
