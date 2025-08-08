using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    
    [Authorize]
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
            var result = await _goalService.AddGoalAsync(goalDto);

            if (result == "InvalidDate")
                return BadRequest(new { message = "StartDate cannot be after EndDate." });

            return Ok(new { message = "Goal added successfully." });
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGoalReport(int userId)
        {
            var jwtUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (jwtUserIdClaim == null || !int.TryParse(jwtUserIdClaim, out var jwtUserId))
                return Unauthorized(new { message = "Invalid or missing token." });

            if (jwtUserId != userId)
                return Forbid("You are not allowed to access other users' goal reports.");

            var report = await _goalService.GetGoalReportAsync(userId);
            return Ok(report);
        }


        [HttpDelete("{goalId}")]
        public async Task<IActionResult> DeleteGoal(int goalId)
        {
            var deletedGoalId = await _goalService.DeleteGoalAsync(goalId);

            return deletedGoalId.HasValue
                ? Ok(new { message = "Goal deleted successfully.", goalId = deletedGoalId.Value })
                : NotFound(new { message = "Goal not found." });
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

        
    }
}
