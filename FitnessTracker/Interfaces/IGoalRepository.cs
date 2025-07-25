using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IGoalRepository
    {
        Task AddGoalAsync(Goal goal);
        Task<List<Goal>> GetGoalsByUserAsync(int userId);
        Task<List<Workout>> GetWorkoutsByUserAsync(int userId);
        Task<Goal?> GetGoalByIdAsync(int goalId);
        Task DeleteGoalAsync(Goal goal);
    }
}
