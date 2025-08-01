using FitnessTracker.GenericRepo;
using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllNonAdminUsersAsync();
        Task<User?> GetUserAsync(int id);
        Task DeleteUserAsync(User user);
        Task SaveChangesAsync();
    }
}
