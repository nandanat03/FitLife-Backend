using FitnessTracker.Dtos;

namespace FitnessTracker.Interfaces
{
    public interface IProgressService
    {
        Task<ProgressDto> GetDailySummaryAsync(int userId);
        Task<ProgressDto> GetMonthlySummaryAsync(int year, int userId);
        Task<ProgressDto> GetCustomSummaryAsync(DateTime start, DateTime end, int userId);

    }
}
