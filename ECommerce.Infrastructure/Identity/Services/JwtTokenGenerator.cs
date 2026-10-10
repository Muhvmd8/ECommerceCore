using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Infrastructure.Identity.Services
{
    public class JwtTokenGenerator(IOptions<JwtSettings> options)
        : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings = options.Value;
        public AccessTokenResult GenerateToken(Guid userId, string email, string? displayName, IEnumerable<string> roles)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes);

            var jti = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            if (!string.IsNullOrWhiteSpace(displayName))
                claims.Add(new Claim(ClaimTypes.Name, displayName));

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var written = new JwtSecurityTokenHandler().WriteToken(token);
            return new AccessTokenResult(written, jti, expiresAt);
        }
    }
}