using FitnessTracker.Models;
using FitnessTracker.GenericRepo;

namespace FitnessTracker.Interfaces
{
    public interface IGoalRepository : IGenericRepository<Goal>
    {
        Task<List<Goal>> GetPaginatedGoalsAsync(int userId, int pageNumber, int pageSize);
        Task<int> GetTotalGoalCountAsync(int userId);
    }
}
