using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services
{
    public class WorkoutService :IWorkoutService
    {
        private readonly UserContext _context;

        public WorkoutService(UserContext context)
        {
            _context = context;
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

            _context.Workouts.Add(workout);
            var result = await _context.SaveChangesAsync();

            return result > 0; // true if at least one row saved
        }

        public async Task<List<Workout>> GetUserWorkoutsAsync(int userId)
        {
            return await _context.Workouts
                .Where(w => w.UserId == userId)
                .OrderBy(w => w.WorkoutDate)
                .ToListAsync();
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            var workout = await _context.Workouts.FindAsync(workoutId);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteMultipleAsync(List<int> workoutIds)
        {
            var workouts = await _context.Workouts
                .Where(w => workoutIds.Contains(w.WorkoutId))
                .ToListAsync();

            if (!workouts.Any())
                return false;

            _context.Workouts.RemoveRange(workouts);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> GetAllActivitiesAsync()
        {
            return await _context.Activities
                .Select(a => new { name = a.ActivityName, metValue = a.MET_Value })
                .ToListAsync<object>();
        }

        public async Task<string> AddActivityAsync(Activity newActivity)
        {
            var exists = await _context.Activities
                .AnyAsync(a => a.ActivityName.ToLower() == newActivity.ActivityName.ToLower());

            if (exists)
                return "Exists";

            _context.Activities.Add(newActivity);
            await _context.SaveChangesAsync();
            return "Success";
        }
    }
}
