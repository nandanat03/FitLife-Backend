using FitnessTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        public readonly UserContext _context;
        public UserController(IConfiguration config, UserContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("CreateUser")]
        public IActionResult Create(User user)
        {
           

             if (_context.Users.Where(u => u.Email == user.Email).FirstOrDefault() != null)
            {
                return Ok(new { message = "AlreadyExist" });
            }
            user.MemberSince = DateTime.Now;
            _context.Users.Add(user);   
            _context.SaveChanges();
            
            return Ok(new { message = "Success" });
        }

        [HttpPost("LoginUser")]

        public IActionResult Login(Login user) {



            var userAvailable = _context.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (userAvailable != null)
            {
                var response = new LoginResponse()
                {
                    Role = userAvailable.Role,
                    UserName = userAvailable.FirstName,
                    UserId = userAvailable.UserId,
                    Weight = userAvailable.Weight
                };

                return Ok(response);
            }
            return Unauthorized("Invalid User");
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = _context.Users
                .Where(u => u.Role != "admin")
                .Select(u => new
                {
                    u.UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Role,
                   
                })
                .ToList();

            return Ok(users);
        }

        
        [HttpDelete("Admin/DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
                return NotFound(new { message = "User not found." });

            if (user.Role == "admin")
                return Forbid("Cannot delete admin users.");

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok(new { message = "User deleted successfully." });
        }
    }

}

