using FitnessTracker.DTOs;

namespace FitnessTracker.Interfaces
{
    public interface IGoalService
    {
        Task<string> AddGoalAsync(GoalDTO dto);
        Task<List<object>> GetGoalReportAsync(int userId);
        Task<bool> DeleteGoalAsync(int goalId);
    }
}
