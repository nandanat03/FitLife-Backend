using FitnessTracker.DTOs;
using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IWorkoutService
    {
        Task<bool> AddWorkoutAsync(WorkoutDTO dto);
        Task<List<Workout>> GetUserWorkoutsAsync(int userId);
        Task DeleteWorkoutAsync(int workoutId);
        Task<bool> DeleteMultipleAsync(List<int> workoutIds);
        Task<List<object>> GetAllActivitiesAsync();
        Task<string> AddActivityAsync(Activity newActivity);

        
    }
}
