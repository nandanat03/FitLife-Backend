
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Repositories
{
    public class ProgressRepository : IProgressRepository
    {
        private readonly UserContext _context;

        public ProgressRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<List<Workout>> GetWorkoutsByDateAsync(DateTime date, int userId)
        {
            return await _context.Workouts
                .Where(w => w.UserId == userId && w.WorkoutDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task<List<Workout>> GetWorkoutsByYearAsync(int year, int userId)
        {
            return await _context.Workouts
                .Where(w => w.UserId == userId && w.WorkoutDate.Year == year)
                .ToListAsync();
        }

        public async Task<List<Workout>> GetWorkoutsBetweenDatesAsync(DateTime start, DateTime end, int userId)
        {
            return await _context.Workouts
                .Where(w => w.UserId == userId && w.WorkoutDate.Date >= start.Date && w.WorkoutDate.Date <= end.Date)
                .ToListAsync();
        }

    }
}
