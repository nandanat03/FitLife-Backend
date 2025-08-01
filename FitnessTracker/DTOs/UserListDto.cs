namespace FitnessTracker.Dtos
{
    public record UserListDto(
        int UserId,
        string FirstName,
        string LastName,
        string Email,
        string Role
    );
}
