using FitnessTracker.Models;
using FitnessTracker.GenericRepo;

namespace FitnessTracker.Interfaces
{
    public interface IWorkoutRepository : IGenericRepository<Workout>
    {
        Task<List<Workout>> GetUserWorkoutsAsync(int userId);
        Task<bool> DeleteWorkoutsAsync(List<Workout> workouts);
        Task<List<object>> GetAllActivitiesAsync();
        Task<bool> ActivityExistsAsync(string activityName);
        Task AddActivityAsync(Activity activity);
    }
}
