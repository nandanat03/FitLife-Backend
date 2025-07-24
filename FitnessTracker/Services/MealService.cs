using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FitnessTracker.Services
{
    public class MealService : IMealService
    {
        private readonly UserContext _context;

        public MealService(UserContext context)
        {
            _context = context;
        }

        public async Task<List<MealDTO>> GetMealsAsync()
        {
            return await _context.Meals
                .Select(m => new MealDTO(
                    m.MealName,
                    m.CaloriesPer100g,
                    m.CaloriesPerPiece
                ))
                .ToListAsync();
        }

        public async Task<bool> AddMealAsync(MealDTO mealDto)
        {
            var meal = new Meal
            {
                MealName = mealDto.Name,
                CaloriesPer100g = mealDto.CaloriesPer100g,
                CaloriesPerPiece = mealDto.CaloriesPerPiece
            };

            _context.Meals.Add(meal);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
