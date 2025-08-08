using FitnessTracker.Dtos;

namespace FitnessTracker.Interfaces
{
    public interface IMealService
    {
        Task<IEnumerable<MealDto>> GetMealsAsync();
        Task<bool> AddMealAsync(MealDto mealDto);
    }
}
