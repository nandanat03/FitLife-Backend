using Asp.Versioning;
using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using FitnessTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class WorkoutController : ControllerBase
{
    private readonly IWorkoutService _workoutService;

    public WorkoutController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }

    [HttpPost("AddWorkout")]
    public async Task<IActionResult> Add([FromBody] WorkoutDto dto)
    {
        var result = await _workoutService.AddWorkoutAsync(dto);

        if (result)
            return Ok(new { message = "Workout added successfully" });

        return BadRequest(new { message = "Failed to add workout." });
    }

    [HttpGet("ViewWorkout/{userId}")]
    public async Task<IActionResult> View(int userId)
    {
        var jwtUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (jwtUserIdClaim == null || !int.TryParse(jwtUserIdClaim, out var jwtUserId))
            return Unauthorized(new { message = "Invalid or missing token." });

        if (jwtUserId != userId)
            return Forbid("You are not allowed to access other users' workouts.");

        var workoutDtos = await _workoutService.GetUserWorkoutsAsync(userId);
        return Ok(workoutDtos);
    }

    [HttpDelete("{workoutId}")]
    public async Task<IActionResult> DeleteWorkout(int workoutId)
    {
        var deletedWorkoutId = await _workoutService.DeleteWorkoutAsync(workoutId);

        return deletedWorkoutId.HasValue
            ? Ok(new { message = "Workout deleted successfully.", workoutId = deletedWorkoutId.Value })
            : NotFound(new { message = "Workout not found." });
    }

    [HttpPost("DeleteMultiple")]
    public async Task<IActionResult> DeleteMultiple([FromBody] List<int> workoutIds)
    {
        var result = await _workoutService.DeleteMultipleAsync(workoutIds);
        return result
            ? NoContent()
            : NotFound(new { message = "No workouts found for given IDs." });
    }

    [HttpGet("GetActivities")]
    public async Task<IActionResult> GetActivities()
    {
        var activities = await _workoutService.GetAllActivitiesAsync();
        return Ok(activities);
    }

    [HttpPost("AddActivity")]
    public async Task<IActionResult> AddActivity([FromBody] ActivityDto newActivity)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _workoutService.AddActivityAsync(newActivity);

        return result switch
        {
            "Success" => Ok(new { message = "Activity added successfully." }),
            "AlreadyExists" => Conflict(new { message = "Activity already exists." }),
            _ => BadRequest(new { message = "Failed to add activity." })
        };
    }
}
