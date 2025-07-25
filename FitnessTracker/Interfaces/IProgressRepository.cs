using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IProgressRepository
    {
        Task<List<Workout>> GetWorkoutsByDateAsync(DateTime date);
        Task<List<Workout>> GetWorkoutsByYearAsync(int year);
        Task<List<Workout>> GetWorkoutsBetweenDatesAsync(DateTime start, DateTime end);
    }
}
