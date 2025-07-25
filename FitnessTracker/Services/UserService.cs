using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace FitnessTracker.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<string> CreateUserAsync(UserCreateDto userDto)
        {
            if (await _userRepository.EmailExistsAsync(userDto.Email))
                return "AlreadyExist";

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Height = userDto.Height,
                Weight = userDto.Weight,
                ActivityLevel = userDto.ActivityLevel,
                MemberSince = DateTime.Now,
                Role = userDto.Email == "nandana@gmail.com" ? "admin" : "user",
                Password = _passwordHasher.HashPassword(null, userDto.Password)
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();
            return "Success";
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
            if (result == PasswordVerificationResult.Failed) return null;

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
            var users = await _userRepository.GetAllNonAdminUsersAsync();

            return users.Select(u => new
            {
                u.UserId,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Role
            }).Cast<object>().ToList();
        }

        public async Task<string> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return "NotFound";

            if (user.Role == "admin") return "CannotDeleteAdmin";

            await _userRepository.DeleteUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return "Deleted";
        }
    }
}
