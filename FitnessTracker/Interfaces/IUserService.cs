using FitnessTracker.Dtos;



namespace FitnessTracker.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(UserCreateDto user);
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<IEnumerable<UserListDto>> GetUsersAsync(); // Non-admin users
        Task<string> DeleteUserAsync(int id);
    }
}
