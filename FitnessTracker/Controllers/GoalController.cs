using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Models;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly UserContext _context;

        public GoalController(UserContext context)
        {
            _context = context;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGoalReport(int userId)
        {
            var goals = await _context.Goals
                                      .Where(g => g.UserId == userId)
                                      .ToListAsync();

            var workouts = await _context.Workouts
                                         .Where(w => w.UserId == userId)
                                         .ToListAsync();

            var report = goals.Select(goal =>
            {
                double achieved = 0;
                DateTime inclusiveEndDate = goal.EndDate.AddDays(1);

                var relevantWorkouts = workouts
                .Where(w => w.WorkoutDate >= goal.StartDate && w.WorkoutDate < inclusiveEndDate);

                if (goal.GoalType == GoalType.Calories)
                {
                    achieved = relevantWorkouts.Sum(w => w.CaloriesBurned);
                }
                else if (goal.GoalType == GoalType.Distance)
                {
                    achieved = relevantWorkouts.Sum(w => w.Distance);
                }

                return new
                {
                    goalId = goal.goalId, // fixed property casing
                    userId = goal.UserId,
                    goalType = goal.GoalType,
                    goalValue = goal.GoalValue,
                    startDate = goal.StartDate,
                    endDate = goal.EndDate,
                    achievedValue = achieved,
                    remaining = Math.Max(goal.GoalValue - achieved, 0)
                };
            });

            return Ok(report);
        }

        [HttpPost]
        public async Task<IActionResult> AddGoal([FromBody] Goal goal)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (goal.StartDate > goal.EndDate)
                return BadRequest("Start date cannot be after end date.");

            if (!Enum.IsDefined(typeof(GoalType), goal.GoalType))
                return BadRequest("Invalid goal type.");

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();

            return Ok(new { message = "GoalAdded" });
        }

        [HttpDelete("{goalId}")]
        public async Task<IActionResult> DeleteGoal(int goalId)
        {
            var goal = await _context.Goals.FindAsync(goalId);
            if (goal == null)
                return NotFound();

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync(); 

            return NoContent();
        }
    }
}
