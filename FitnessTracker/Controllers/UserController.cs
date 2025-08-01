using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto user)
        {
            var result = await _userService.CreateUserAsync(user);
            return result switch
            {
                "Success" => Ok(new { message = "User created successfully." }),
                "AlreadyExist" => Conflict(new { message = "Email already exists." }),
                _ => throw new Exception("Internal error creating user.")
            };
        }



        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var result = await _userService.LoginAsync(login);

            if (result == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(result); 
        }
        // Get list of non-admin users
        
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        // Admin delete
        [Authorize(Roles = "admin")]
        [HttpDelete("Admin/DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            return result switch
            {
                "NotFound" => NotFound(new { message = "User not found." }),
                "CannotDeleteAdmin" => Forbid("Cannot delete admin users."),
                "Deleted" => Ok(new { message = "User deleted successfully." }),
                _ => StatusCode(500, new { message = "Unknown error occurred." })
            };
        }
    }
}
