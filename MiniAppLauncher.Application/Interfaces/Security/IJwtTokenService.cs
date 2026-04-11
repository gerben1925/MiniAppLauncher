using MiniAppLauncher.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAppLauncher.Application.Interfaces.Security
{
    public interface IJwtTokenService
    {
        public string GenerateAccessToken(UserEntity user);
        public string GenerateRefreshToken();
        public DateTime GetAccessTokenExpiration();
        public DateTime GetRefreshTokenExpiration();
    }
}
