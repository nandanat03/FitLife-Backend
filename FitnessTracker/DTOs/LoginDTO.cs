using System.ComponentModel.DataAnnotations;
using FitnessTracker.DTOs.SmartRequiredAttribute;

namespace FitnessTracker.Dtos
{
    public class LoginDto
    {
        [Smart]
        [EmailAddress]
        public string? Email { get; set; }

        [Smart]
        public string? Password { get; set; }
    }
}
 
