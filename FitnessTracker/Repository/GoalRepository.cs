using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.GenericRepo;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Repositories
{
    public class GoalRepository : GenericRepository<Goal>, IGoalRepository
    {
        private readonly UserContext _context;

        public GoalRepository(UserContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Goal>> GetPaginatedGoalsAsync(int userId, int pageNumber, int pageSize)
        {
            return await _context.Goals
                .Where(g => g.UserId == userId)
                .OrderByDescending(g => g.StartDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalGoalCountAsync(int userId)
        {
            return await _context.Goals.CountAsync(g => g.UserId == userId);
        }
    }
}
