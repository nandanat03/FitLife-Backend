using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using System.Security.Claims;

namespace FitnessTracker.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtService _jwtService;

        public TokenService(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public RefreshTokenDto GenerateTokens(string email, string role)
        {
            var (accessToken, refreshToken) = _jwtService.GenerateTokens(email, role);
            return new RefreshTokenDto(accessToken, refreshToken);
        }

        public RefreshTokenDto? RefreshAccessToken(string refreshToken)
        {
            var principal = _jwtService.ValidateToken(refreshToken);
            if (principal == null)
                return null;

            var isRefresh = principal.Claims.FirstOrDefault(c => c.Type == "Refresh")?.Value == "true";
            if (!isRefresh)
                return null;

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var role = principal.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role))
                return null;

            return GenerateTokens(email, role);
        }
    }
}