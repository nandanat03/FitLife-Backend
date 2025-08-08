using Asp.Versioning;
using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MealController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        
        [HttpGet("GetMealCalories")]
        public async Task<ActionResult<IEnumerable<MealDto>>> GetMealCalories()
        {
            var result = await _mealService.GetMealsAsync();
            return Ok(result);
        }

        
        [HttpPost("Admin/AddMeal")]
        public async Task<IActionResult> AddMeal([FromBody] MealDto mealDto)
        {
            var result = await _mealService.AddMealAsync(mealDto);
            return Ok(new { message = "Meal added successfully." });
        }
    }
}
