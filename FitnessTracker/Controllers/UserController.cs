using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserCreateDto user)
        {
            var result = await _userService.CreateUserAsync(user);
            return Ok(new { message = result });
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var result = await _userService.LoginAsync(login);

            if (result == null)
                return Unauthorized(new { message = "Invalid User" });

            return Ok(result);
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpDelete("Admin/DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            return result switch
            {
                "NotFound" => NotFound(new { message = "User not found." }),
                "CannotDeleteAdmin" => Forbid("Cannot delete admin users."),
                "Deleted" => Ok(new { message = "User deleted successfully." }),
                _ => StatusCode(500, "Unknown error")
            };
        }
    }
}
