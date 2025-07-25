
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private readonly UserContext _context;

        public GoalRepository(UserContext context)
        {
            _context = context;
        }

        public async Task AddGoalAsync(Goal goal)
        {
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Goal>> GetGoalsByUserAsync(int userId)
        {
            return await _context.Goals
                .Where(g => g.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Workout>> GetWorkoutsByUserAsync(int userId)
        {
            return await _context.Workouts
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<Goal?> GetGoalByIdAsync(int goalId)
        {
            return await _context.Goals.FindAsync(goalId);
        }

        public async Task DeleteGoalAsync(Goal goal)
        {
            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
        }
    }
}
