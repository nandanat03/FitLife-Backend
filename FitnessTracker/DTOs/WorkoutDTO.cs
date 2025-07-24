using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.DTOs
{
    public record WorkoutDTO
    {
        [Required]
        public int UserId { get; init; }

        [Required]
        public string? ActivityType { get; init; }

        [Required]
        public int Duration { get; init; }

        [Required]
        public int Distance { get; init; }

        public double CaloriesBurned { get; init; }

        [Required]
        public DateTime WorkoutDate { get; init; }
    }
}
