using FitnessTracker.Dtos;
using FitnessTracker.Services;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.UnitOfWork;
using Microsoft.AspNetCore.Identity;


namespace FitnessTracker.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly JwtService _jwtService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, JwtService jwtService, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _userRepo = _unitOfWork.Users as IUserRepository
                ?? throw new InvalidOperationException("Users repository is not an IUserRepository.");
            _passwordHasher = new PasswordHasher<User>();
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<string> CreateUserAsync(UserCreateDto userDto)
        {
            _logger.LogInformation("Attempting to register user with email: {Email}", userDto.Email);

            if (await _userRepo.EmailExistsAsync(userDto.Email))
            {
                _logger.LogWarning("Registration failed: email {Email} already exists", userDto.Email);
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
                Password = _passwordHasher.HashPassword(new User(), userDto.Password)

            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User registered successfully: {Email}, Role: {Role}", user.Email, user.Role);

            return "Success";
        }

       public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
{
    _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            var user = await _userRepo.GetUserByEmailAsync(loginDto.Email!);

            if (user == null)
    {
        _logger.LogWarning("Login failed: email not found - {Email}", loginDto.Email);
        return null;
    }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password!);

            if (result == PasswordVerificationResult.Failed)
    {
        _logger.LogWarning("Login failed: invalid password for email {Email}", loginDto.Email);
        return null;
    }

    _logger.LogInformation("Login successful for email {Email}", loginDto.Email);

          var tokens = _jwtService.GenerateTokens(user.Email, user.Role, user.UserId);


            return new LoginResponseDto
    {
        UserId = user.UserId,
        UserName = user.FirstName!,
        Role = user.Role,
        Weight = user.Weight,
        Token = tokens.AccessToken,
        RefreshToken = tokens.RefreshToken
    };
}


        public async Task<IEnumerable<UserListDto>> GetUsersAsync()
        {
            _logger.LogInformation("Fetching non-admin users");

            var users = await _userRepo.GetAllNonAdminUsersAsync();

            _logger.LogInformation("{Count} users fetched", users.Count);

            return users.Select(u => new UserListDto(
                u!.UserId,
                u!.FirstName,
                u!.LastName,
                u!.Email,
                u!.Role
            )); 
        }



        public async Task<string> DeleteUserAsync(int id)
        {
            _logger.LogInformation("Attempting to delete user with ID: {UserId}", id);

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Delete failed: user with ID {UserId} not found", id);
                return "NotFound";
            }

            if (user.Role == "admin")
            {
                _logger.LogWarning("Delete blocked: user with ID {UserId} is admin", id);
                return "CannotDeleteAdmin";
            }

            await _unitOfWork.Users.DeleteAsync(user);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User deleted successfully: ID {UserId}", id);

            return "Deleted";
        }
    }
}
