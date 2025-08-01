using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;

        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpPost]
        public async Task<IActionResult> AddGoal([FromBody] GoalDto goalDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _goalService.AddGoalAsync(goalDto);
            return result == "Success"
                ? Ok(new { message = "Goal added successfully" })
                : throw new Exception("Failed to add goal");
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
            var deletedGoalId = await _goalService.DeleteGoalAsync(goalId);
            return deletedGoalId.HasValue
                ? Ok(new { message = "Goal deleted successfully", goalId = deletedGoalId.Value })
                : NotFound(new { message = "Goal not found" });
        }

        [HttpGet("user/{userId}/paginated")]
        public async Task<IActionResult> GetPaginatedGoals(int userId, int page = 1, int pageSize = 10)
        {
            var (goals, totalCount) = await _goalService.GetGoalsPaginatedAsync(userId, page, pageSize);

            return Ok(new
            {
                totalCount,
                currentPage = page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                data = goals
            });
        }

        [HttpGet("error")]
        public IActionResult TriggerError()
        {
            throw new Exception("This is a test exception!");
        }
    }
}
