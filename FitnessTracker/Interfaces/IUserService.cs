using FitnessTracker.DTOs;


namespace FitnessTracker.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(UserCreateDto user);
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<List<object>> GetUsersAsync(); // Non-admin users
        Task<string> DeleteUserAsync(int id);
    }
}
