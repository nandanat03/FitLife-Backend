using FitnessTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly UserContext _context;

        public MealController(UserContext context)
        {
            _context = context;
        }

        [HttpGet("GetMealCalories")]   
        public IActionResult GetMealCalories()
        {
            var foodCalories = _context.Meals
                .Select(f => new
                {
                    name = f.MealName,
                    caloriesPer100g = f.CaloriesPer100g,      
                    caloriesPerPiece = f.CaloriesPerPiece
                })
                .ToList();

            return Ok(foodCalories);
        }

        [HttpPost("Admin/AddMeal")]
        public IActionResult AddMeal([FromBody] Meal meal)
        {
            if (meal == null || string.IsNullOrWhiteSpace(meal.MealName))
                return BadRequest("Invalid meal data.");

            _context.Meals.Add(meal);
            _context.SaveChanges();

            return Ok(new { message = "Meal added successfully." });
        }

    }
}
