using FitnessTracker.Dtos;
using FitnessTracker.Models;
using FitnessTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutRepo;

        public WorkoutController(IWorkoutService workoutRepo)
        {
            _workoutRepo = workoutRepo;
        }

        [HttpPost("AddWorkout")]
        public async Task<IActionResult> Add([FromBody] WorkoutDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _workoutRepo.AddWorkoutAsync(dto);
            return result
                ? Ok(new { message = "Workout added successfully" })
                : throw new Exception("Failed to add workout");
        }

        [HttpGet("ViewWorkout/{userId}")]
        public async Task<IActionResult> View(int userId)
        {
            var workoutDtos = await _workoutRepo.GetUserWorkoutsAsync(userId);
            return Ok(workoutDtos);
        }

        [HttpDelete("{workoutId}")]
        public async Task<IActionResult> DeleteWorkout(int workoutId)
        {
            await _workoutRepo.DeleteWorkoutAsync(workoutId);
            return NoContent();
        }

        [HttpPost("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<int> workoutIds)
        {
            var result = await _workoutRepo.DeleteMultipleAsync(workoutIds);
            return result
                ? NoContent()
                : NotFound("No workouts found for given IDs");
        }

        [HttpGet("GetActivities")]
        public async Task<IActionResult> GetActivities()
        {
            var activities = await _workoutRepo.GetAllActivitiesAsync();
            return Ok(activities);
        }

        [HttpPost("AddActivity")]
        public async Task<IActionResult> AddActivity([FromBody] ActivityDto newActivity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _workoutRepo.AddActivityAsync(newActivity);
            return result == "Success"
                ? Ok(new { message = "Activity added successfully" })
                : throw new Exception("Failed to add activity");
        }
    }
}
