using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace FitnessTracker.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepo = _unitOfWork.Users as IUserRepository
                ?? throw new InvalidOperationException("Users repository is not an IUserRepository.");
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<string> CreateUserAsync(UserCreateDto userDto)
        {
            Log.Information("Attempting to register user with email: {Email}", userDto.Email);

            if (await _userRepo.EmailExistsAsync(userDto.Email))
            {
                Log.Warning("Registration failed: email {Email} already exists", userDto.Email);
                return "AlreadyExist";
            }

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

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAsync();

            Log.Information("User registered successfully: {Email}, Role: {Role}", user.Email, user.Role);

            return "Success";
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            Log.Information("Login attempt for email: {Email}", loginDto.Email);

            var user = await _userRepo.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                Log.Warning("Login failed: email not found - {Email}", loginDto.Email);
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                Log.Warning("Login failed: invalid password for email {Email}", loginDto.Email);
                return null;
            }

            Log.Information("Login successful for email {Email}", loginDto.Email);

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
            Log.Information("Fetching non-admin users");

            var users = await _userRepo.GetAllNonAdminUsersAsync();

            Log.Information("{Count} users fetched", users.Count);

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
            Log.Information("Attempting to delete user with ID: {UserId}", id);

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                Log.Warning("Delete failed: user with ID {UserId} not found", id);
                return "NotFound";
            }

            if (user.Role == "admin")
            {
                Log.Warning("Delete blocked: user with ID {UserId} is admin", id);
                return "CannotDeleteAdmin";
            }

            await _unitOfWork.Users.DeleteAsync(user);
            await _unitOfWork.SaveAsync();

            Log.Information("User deleted successfully: ID {UserId}", id);

            return "Deleted";
        }
    }
}
