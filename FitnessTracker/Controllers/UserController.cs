using Asp.Versioning;
using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto user)
        {
            _logger.LogInformation("CreateUser attempt: {Email}", user.Email);

            var result = await _userService.CreateUserAsync(user);

            return result switch
            {
                "Success" => Ok(new { message = "CreatedSuccessfully" }),
                "AlreadyExist" => Conflict(new { message = "AlreadyExists" }),
                _ => BadRequest(new { message = "Could not create user due to an unknown error." })
            };
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            _logger.LogInformation("Login attempt for: {Email}", login.Email);

            var result = await _userService.LoginAsync(login);

            if (result == null)
            {
                _logger.LogWarning("Failed login for {Email}", login.Email);
                return Unauthorized(new { message = "Invalid email or password." });
            }


            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("Admin/DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("DeleteUser attempt for ID: {Id}", id);

            var result = await _userService.DeleteUserAsync(id);

            return result switch
            {
                "NotFound" => NotFound(new { message = "User not found." }),
                "CannotDeleteAdmin" => Forbid("Cannot delete admin users."),
                "Deleted" => Ok(new { message = "User deleted successfully." }),
                _ => BadRequest(new { message = "Could not delete user due to unknown reason." })
            };
        }

      
    }
}
