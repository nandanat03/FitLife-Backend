using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;

namespace FitnessTracker.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _repo;

        public WorkoutService(IWorkoutRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> AddWorkoutAsync(WorkoutDTO dto)
        {
            var workout = new Workout
            {
                ActivityType = dto.ActivityType,
                Duration = dto.Duration,
                Distance = dto.Distance,
                CaloriesBurned = dto.CaloriesBurned,
                WorkoutDate = dto.WorkoutDate,
                UserId = dto.UserId
            };

            return await _repo.AddWorkoutAsync(workout);
        }

        public async Task<List<Workout>> GetUserWorkoutsAsync(int userId)
        {
            return await _repo.GetUserWorkoutsAsync(userId);
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            var workout = await _repo.GetWorkoutByIdAsync(workoutId);
            if (workout != null)
            {
                await _repo.DeleteWorkoutsAsync(new List<Workout> { workout });
            }
        }

        public async Task<bool> DeleteMultipleAsync(List<int> workoutIds)
        {
            var workouts = await _repo.GetUserWorkoutsAsync(0); // to get all, but we filter manually
            var selected = workouts.Where(w => workoutIds.Contains(w.WorkoutId)).ToList();
            if (!selected.Any()) return false;

            return await _repo.DeleteWorkoutsAsync(selected);
        }

        public async Task<List<object>> GetAllActivitiesAsync()
        {
            return await _repo.GetAllActivitiesAsync();
        }

        public async Task<string> AddActivityAsync(Activity newActivity)
        {
            if (await _repo.ActivityExistsAsync(newActivity.ActivityName))
                return "Exists";

            await _repo.AddActivityAsync(newActivity);
            return "Success";
        }
    }
}
