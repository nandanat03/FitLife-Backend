using Microsoft.AspNetCore.Mvc;
using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;

        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpPost]
        public async Task<IActionResult> AddGoal([FromBody] GoalDTO goalDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _goalService.AddGoalAsync(goalDto);
            return result == "Success"
                ? Ok(new { message = "Goal added successfully" })
                : StatusCode(500, new { message = "Failed to add goal" });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGoalReport(int userId)
        {
            var report = await _goalService.GetGoalReportAsync(userId);
            return Ok(report);
        }

        [HttpDelete("{goalId}")]
        public async Task<IActionResult> DeleteGoal(int goalId)
        {
            var result = await _goalService.DeleteGoalAsync(goalId);
            return result
                ? NoContent()
                : NotFound(new { message = "Goal not found" });
        }
    }
}
