using AutoMapper;
using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.UnitOfWork;

namespace FitnessTracker.Services
{
    public class MealService : IMealService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MealService> _logger;

        public MealService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MealService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MealDto>> GetMealsAsync()
        {
            _logger.LogInformation("Fetching all meals from the database.");

            var meals = await _unitOfWork.Meals.GetAllAsync();

            _logger.LogInformation("Fetched {Count} meals.", meals.Count());

            return meals.Select(m => new MealDto
            {
                Name = m.MealName,
                CaloriesPer100g = m.CaloriesPer100g,
                CaloriesPerPiece = m.CaloriesPerPiece
            }); 
        }

        public async Task<bool> AddMealAsync(MealDto mealDto)
        {
            _logger.LogInformation("Attempting to add new meal: {Name}", mealDto.Name);

            var meal = new Meal
            {
                MealName = mealDto.Name,
                CaloriesPer100g = mealDto.CaloriesPer100g,
                CaloriesPerPiece = mealDto.CaloriesPerPiece
            };

            await _unitOfWork.Meals.AddAsync(meal);
            var result = await _unitOfWork.SaveAsync();

            if (result > 0)
                _logger.LogInformation("Meal added successfully: {Name}", mealDto.Name);
            else
                _logger.LogWarning("Failed to add meal: {Name}", mealDto.Name);

            return result > 0;
        }
    }
}
