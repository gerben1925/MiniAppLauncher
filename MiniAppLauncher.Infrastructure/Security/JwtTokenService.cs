using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniAppLauncher.Application.Interfaces.Security;
using MiniAppLauncher.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MiniAppLauncher.Infrastructure.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GenerateAccessToken(UserEntity user)
        {

            var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT:SecretKey is missing");
            var issuer = _config["JWT:Issuer"] ?? throw new InvalidOperationException("JWT:Issuer is missing");
            var audience = _config["JWT:Audience"] ?? throw new InvalidOperationException("JWT:Audience is missing");
            var expirationMinutes = _config.GetValue<double>("JWT:AccessTokenExpiryMinutes");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[]
             {
                new Claim(ClaimTypes.Sid, user.UserID.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserReference.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.GivenName, $"{user.LastName}, {user.FirstName}"),
                new Claim(ClaimTypes.Role, user.RoleID.ToString())
            };

            var accessToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public DateTime GetAccessTokenExpiration()
        {
            var expiryMinutes = int.Parse(_config["JWT:AccessTokenExpiryMinutes"] ?? throw new InvalidOperationException("JWT:AccessTokenExpiryMinutes is missing."));
            return DateTime.UtcNow.AddMinutes(expiryMinutes);
        }

        public DateTime GetRefreshTokenExpiration()
        {
            var expiryDays = int.Parse(_config["JWT:RefreshTokenExpiresByDays"] ?? throw new InvalidOperationException("JWT:RefreshTokenExpiresByDays is missing."));
            return DateTime.UtcNow.AddDays(expiryDays);
        }

    }
}
