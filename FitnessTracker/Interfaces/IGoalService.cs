using FitnessTracker.Dtos;
using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IGoalService
    {
        Task<string> AddGoalAsync(GoalDto dto);
        Task<IEnumerable<GoalReportDto>> GetGoalReportAsync(int userId);
        Task<int?> DeleteGoalAsync(int goalId);
        Task<(List<GoalDto> goals, int totalCount)> GetGoalsPaginatedAsync(int userId, int page, int pageSize);



    }
}
