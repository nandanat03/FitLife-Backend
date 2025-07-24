using FitnessTracker.DTOs;

namespace FitnessTracker.Interfaces
{
    public interface IMealService
    {
        Task<List<MealDTO>> GetMealsAsync();
        Task<bool> AddMealAsync(MealDTO mealDto);
    }
}
