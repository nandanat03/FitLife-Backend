using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = _configuration["JwtSettings:Key"]!;
            _issuer = _configuration["JwtSettings:Issuer"]!;
            _audience = _configuration["JwtSettings:Audience"]!;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims, DateTime expiry)
        {
            return new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expiry,
                signingCredentials: GetSigningCredentials()
            );
        }

        public string GenerateToken(string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
            };

            var token = CreateJwtToken(claims, DateTime.UtcNow.AddMinutes(15));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("Refresh", "true")
            };

            var token = CreateJwtToken(claims, DateTime.UtcNow.AddDays(5));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string AccessToken, string RefreshToken) GenerateTokens(string email, string role)
        {
            var accessToken = GenerateToken(email, role);
            var refreshToken = GenerateRefreshToken(email, role);
            return (accessToken, refreshToken);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var keyBytes = Encoding.UTF8.GetBytes(_key);

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}