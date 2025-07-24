namespace FitnessTracker.Interfaces
{
    public interface IProgressService
    {
        Task<object> GetDailySummaryAsync();
        Task<object> GetMonthlySummaryAsync(int year);
        Task<object> GetCustomSummaryAsync(DateTime start, DateTime end);
    }
}
