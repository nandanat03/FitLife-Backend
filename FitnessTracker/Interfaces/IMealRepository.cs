using FitnessTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessTracker.Interfaces
{
    public interface IMealRepository
    {
        Task<List<Meal>> GetAllMealsAsync();
        Task<bool> AddMealAsync(Meal meal);
    }
}
