using FitnessTracker.GenericRepo;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Repositories
{
    public class WorkoutRepository : GenericRepository<Workout>, IWorkoutRepository
    {
        private readonly UserContext _context;

        public WorkoutRepository(UserContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Workout>> GetUserWorkoutsAsync(int userId)
        {
            return await _context.Workouts
                .Where(w => w.UserId == userId)
                .OrderBy(w => w.WorkoutDate)
                .ToListAsync();
        }

        public async Task<bool> DeleteWorkoutsAsync(List<Workout> workouts)
        {
            _context.Workouts.RemoveRange(workouts);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<object>> GetAllActivitiesAsync()
        {
            return await _context.Activities
                .Select(a => new { name = a.ActivityName, metValue = a.MET_Value })
                .ToListAsync<object>();
        }

        public async Task<bool> ActivityExistsAsync(string activityName)
        {
            return await _context.Activities
                .AnyAsync(a => a.ActivityName.ToLower() == activityName.ToLower());
        }

        public async Task AddActivityAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }
    }
}
