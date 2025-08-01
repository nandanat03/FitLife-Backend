namespace FitnessTracker.Dtos
{
    public record RefreshTokenDto(
        string AccessToken, 
        string RefreshToken
        );
}
