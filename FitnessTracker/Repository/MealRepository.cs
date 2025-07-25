using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Repositories
{
    public class MealRepository : IMealRepository
    {
        private readonly UserContext _context;

        public MealRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<List<Meal>> GetAllMealsAsync()
        {
            return await _context.Meals.ToListAsync();
        }

        public async Task<bool> AddMealAsync(Meal meal)
        {
            _context.Meals.Add(meal);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
