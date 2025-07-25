using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;

        public MealService(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<List<MealDTO>> GetMealsAsync()
        {
            var meals = await _mealRepository.GetAllMealsAsync();

            return meals.Select(m => new MealDTO
            {
                Name = m.MealName,
                CaloriesPer100g = m.CaloriesPer100g,
                CaloriesPerPiece = m.CaloriesPerPiece
            }).ToList();
        }

        public async Task<bool> AddMealAsync(MealDTO mealDto)
        {
            var meal = new Meal
            {
                MealName = mealDto.Name,
                CaloriesPer100g = mealDto.CaloriesPer100g,
                CaloriesPerPiece = mealDto.CaloriesPerPiece
            };

            return await _mealRepository.AddMealAsync(meal);
        }
    }
}
