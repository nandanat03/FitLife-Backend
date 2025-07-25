using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IWorkoutRepository
    {
        Task<bool> AddWorkoutAsync(Workout workout);
        Task<List<Workout>> GetUserWorkoutsAsync(int userId);
        Task<Workout?> GetWorkoutByIdAsync(int workoutId);
        Task<bool> DeleteWorkoutsAsync(List<Workout> workouts);
        Task<List<object>> GetAllActivitiesAsync();
        Task<bool> ActivityExistsAsync(string activityName);
        Task AddActivityAsync(Activity activity);
    }
}
