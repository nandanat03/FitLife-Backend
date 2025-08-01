using FitnessTracker.Dtos;

namespace FitnessTracker.Interfaces
{
    public interface ITokenService
    {
        RefreshTokenDto GenerateTokens(string email, string role);
        RefreshTokenDto? RefreshAccessToken(string refreshToken);
    }
}
