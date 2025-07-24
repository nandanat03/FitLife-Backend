using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpGet("GetMealCalories")]
        [ProducesResponseType(typeof(IEnumerable<MealDTO>), 200)]
        public async Task<ActionResult<IEnumerable<MealDTO>>> GetMealCalories()
        {
            var result = await _mealService.GetMealsAsync();
            return Ok(result);
        }

        [HttpPost("Admin/AddMeal")]
        public async Task<IActionResult> AddMeal([FromBody] MealDTO mealDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mealService.AddMealAsync(mealDto);

            return result
                ? Ok(new { message = "Meal added successfully." })
                : StatusCode(500, new { message = "Failed to add meal." });
        }
    }
}
