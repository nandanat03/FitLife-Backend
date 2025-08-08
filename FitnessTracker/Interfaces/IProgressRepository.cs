using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IProgressRepository
    {
        Task<List<Workout>> GetWorkoutsByDateAsync(DateTime date, int userId);
        Task<List<Workout>> GetWorkoutsByYearAsync(int year, int userId);
        Task<List<Workout>> GetWorkoutsBetweenDatesAsync(DateTime start, DateTime end, int userId);

    }
}
