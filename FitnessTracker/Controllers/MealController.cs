using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MealController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpGet("GetMealCalories")]
        [ProducesResponseType(typeof(IEnumerable<MealDto>), 200)]
        public async Task<ActionResult<IEnumerable<MealDto>>> GetMealCalories()
        {
            var result = await _mealService.GetMealsAsync();
            return Ok(result);
        }

        [HttpPost("Admin/AddMeal")]
        public async Task<IActionResult> AddMeal([FromBody] MealDto mealDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mealService.AddMealAsync(mealDto);

            return result
                ? Ok(new { message = "Meal added successfully." })
                : throw new Exception("Failed to add meal.");
        }
    }
}
