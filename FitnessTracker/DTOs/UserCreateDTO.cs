using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.DTOs
{
    public record UserCreateDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string Role { get; set; } = "user";

        public double Height { get; set; }

        public double Weight { get; set; }

        public string ActivityLevel { get; set; }
    }
}
