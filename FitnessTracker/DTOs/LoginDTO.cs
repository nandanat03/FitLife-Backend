using System.ComponentModel.DataAnnotations;
using FitnessTracker.DTOs.SmartRequiredAttribute;

namespace FitnessTracker.Dtos
{
    public record LoginDto
    {
        [Smart]
        [EmailAddress]
        public string? Email { get; set; }

        [Smart]
        public string? Password { get; set; }
    }
}
