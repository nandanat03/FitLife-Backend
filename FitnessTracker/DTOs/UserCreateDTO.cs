using FitnessTracker.DTOs.SmartRequiredAttribute;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Dtos
{
    public record UserCreateDto
    {
        [Smart]
        public string FirstName { get; set; }

        [Smart]
        public string LastName { get; set; }

        [Smart]
        [EmailAddress]
        public string Email { get; set; }

        [Smart]
        [MinLength(6)]
        public string Password { get; set; }

        public string Role { get; set; } = "user";

        public double Height { get; set; }

        public double Weight { get; set; }

        public string ActivityLevel { get; set; }
    }
}
