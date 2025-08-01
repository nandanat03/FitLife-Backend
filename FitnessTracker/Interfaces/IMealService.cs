﻿using FitnessTracker.Dtos;

namespace FitnessTracker.Interfaces
{
    public interface IMealService
    {
        Task<List<MealDto>> GetMealsAsync();
        Task<bool> AddMealAsync(MealDto mealDto);
    }
}
