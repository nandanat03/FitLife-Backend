using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;

        public UserService(UserContext context)
        {
            _context = context;
        }

        public async Task<string> CreateUserAsync(UserCreateDto userDto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == userDto.Email);
            if (exists)
                return "AlreadyExist";

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                Height = userDto.Height,
                Weight = userDto.Weight,
                ActivityLevel = userDto.ActivityLevel,
                MemberSince = DateTime.Now,
                Role = userDto.Email == "nandana@gmail.com" ? "admin" : "user"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Password == loginDto.Password);

            if (user == null) return null;

            return new LoginResponseDto
            {
                UserId = user.UserId,
                UserName = user.FirstName,
                Role = user.Role,
                Weight = user.Weight
            };
        }

        public async Task<List<object>> GetUsersAsync()
        {
            var users = await _context.Users
                .Where(u => u.Role != "admin")
                .Select(u => new
                {
                    u.UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Role
                })
                .ToListAsync();

            return users.Cast<object>().ToList();
        }

        public async Task<string> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return "NotFound";

            if (user.Role == "admin") return "CannotDeleteAdmin";

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return "Deleted";
        }
    }
}
