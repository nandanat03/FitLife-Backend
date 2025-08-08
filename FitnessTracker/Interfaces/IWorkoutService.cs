using FitnessTracker.Dtos;
using FitnessTracker.Models;

namespace FitnessTracker.Interfaces
{
    public interface IWorkoutService
    {
        Task<bool> AddWorkoutAsync(WorkoutDto dto);
        Task<IEnumerable<Workout>> GetUserWorkoutsAsync(int userId);
        Task<int?> DeleteWorkoutAsync(int workoutId);
        Task<bool> DeleteMultipleAsync(List<int> workoutIds);
        Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync();
        Task<string> AddActivityAsync(ActivityDto newActivity);

        
    }
}
