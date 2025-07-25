using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllNonAdminUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task DeleteUserAsync(User user);
        Task SaveChangesAsync();
    }
}
