using FitnessTracker.DTOs;
using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(UserCreateDto user);
        Task<LoginResponseDto?> LoginAsync(LoginDto logindto);
        Task<List<object>> GetUsersAsync();
        Task<string> DeleteUserAsync(int id);
    }
}
