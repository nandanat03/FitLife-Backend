using FitnessTracker.Models;
using Microsoft.AspNetCore.Mvc;


namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IConfiguration _config;
        public readonly UserContext _context;

        public WorkoutController(IConfiguration config, UserContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("AddWorkout")]
        public IActionResult Add([FromBody] Workout workout)
        {
            if (workout == null || string.IsNullOrEmpty(workout.ActivityType) || workout.Duration <= 0)
            {
                return BadRequest(new { message = "Invalid workout data" });
            }

            try
            {
                var workoutpost = new Workout
                {
                    ActivityType = workout.ActivityType,
                    CaloriesBurned = workout.CaloriesBurned,
                    Distance = workout.Distance,
                    Duration = workout.Duration,
                    UserId = workout.UserId,
                    WorkoutDate = workout.WorkoutDate
                    
                };
                _context.Workouts.Add(workoutpost);
                _context.SaveChanges();
                return Ok(new { message = "WorkoutAdded" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error", error = ex.Message });
            }
        }

        [HttpGet("ViewWorkout/{userId}")]
        public IActionResult View(int userId)
        {
            var workouts = _context.Workouts
        .Where(w => w.UserId == userId)
        .OrderBy(w => w.WorkoutDate)
        .ToList();

            return Ok(workouts);
        }

        [HttpDelete("{workoutId}")]
        public IActionResult DeleteWorkout(int workoutId)
        {
            var workout = _context.Workouts.Find(workoutId);
            if (workout == null)
                return NotFound();

            _context.Workouts.Remove(workout);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost("DeleteMultiple")]
        public IActionResult DeleteMultiple([FromBody] List<int> workoutIds)
        {
            var workouts = _context.Workouts
                .Where(w => workoutIds.Contains(w.WorkoutId))
                .ToList();

            if (!workouts.Any())
                return NotFound("No workouts found for the given IDs.");

            _context.Workouts.RemoveRange(workouts);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("GetActivities")]
        public IActionResult GetActivities()
        {
            var activities = _context.Activities
                .Select(a => new
                {
                    name = a.ActivityName,
                    metValue = a.MET_Value
                })
                .ToList();

            return Ok(activities);
        }

        [HttpPost("Admin/AddActivity")]
        public IActionResult AddActivity([FromBody] Activity newActivity)
        {
            if (newActivity == null || string.IsNullOrWhiteSpace(newActivity.ActivityName) || newActivity.MET_Value <= 0)
            {
                return BadRequest(new { message = "Invalid activity data" });
            }

            
            var existing = _context.Activities
                .FirstOrDefault(a => a.ActivityName.ToLower() == newActivity.ActivityName.ToLower());

            if (existing != null)
            {
                return Conflict(new { message = "Activity already exists" });
            }

            try
            {
                _context.Activities.Add(newActivity);
                _context.SaveChanges();
                return Ok(new { message = "Activity added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error adding activity", error = ex.Message });
            }
        }
    }
}
