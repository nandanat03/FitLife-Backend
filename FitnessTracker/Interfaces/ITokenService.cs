using FitnessTracker.Dtos;

namespace FitnessTracker.Interfaces
{
    public interface ITokenService
    {
        RefreshTokenDto GenerateTokens(string email, string role, int userId);
        RefreshTokenDto? RefreshAccessToken(string refreshToken);
    }
}
