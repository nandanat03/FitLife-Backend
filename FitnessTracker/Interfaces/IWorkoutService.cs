using FitnessTracker.Dtos;
using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IWorkoutService
    {
        Task<bool> AddWorkoutAsync(WorkoutDto dto);
        Task<List<Workout>> GetUserWorkoutsAsync(int userId);
        Task DeleteWorkoutAsync(int workoutId);
        Task<bool> DeleteMultipleAsync(List<int> workoutIds);
        Task<List<ActivityDto>> GetAllActivitiesAsync();
        Task<string> AddActivityAsync(ActivityDto newActivity);

        
    }
}
