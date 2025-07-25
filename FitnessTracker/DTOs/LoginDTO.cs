using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.DTOs
{
    public record LoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
