using AutoMapper;
using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.UnitOfWork;
using Serilog;

namespace FitnessTracker.Services
{
    public class MealService : IMealService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MealService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MealDTO>> GetMealsAsync()
        {
            Log.Information("Fetching all meals from the database.");

            var meals = await _unitOfWork.Meals.GetAllAsync();

            Log.Information("Fetched {Count} meals.", meals.Count());

            return meals.Select(m => new MealDTO
            {
                Name = m.MealName,
                CaloriesPer100g = m.CaloriesPer100g,
                CaloriesPerPiece = m.CaloriesPerPiece
            }).ToList();
        }

        public async Task<bool> AddMealAsync(MealDTO mealDto)
        {
            Log.Information("Attempting to add new meal: {Name}", mealDto.Name);

            var meal = new Meal
            {
                MealName = mealDto.Name,
                CaloriesPer100g = mealDto.CaloriesPer100g,
                CaloriesPerPiece = mealDto.CaloriesPerPiece
            };

            await _unitOfWork.Meals.AddAsync(meal);
            var result = await _unitOfWork.SaveAsync();

            if (result > 0)
                Log.Information("Meal added successfully: {Name}", mealDto.Name);
            else
                Log.Warning("Failed to add meal: {Name}", mealDto.Name);

            return result > 0;
        }
    }
}
