using System.ComponentModel.DataAnnotations;
using FitnessTracker.DTOs.SmartRequiredAttribute;

namespace FitnessTracker.Dtos
{
    public record WorkoutDto
    {
        public int WorkoutId { get; init; }

        [Smart]
        public int UserId { get; init; }

        [Smart]
        public string? ActivityType { get; init; }

        [Smart]
        public int Duration { get; init; }

        [Smart]
        public int Distance { get; init; }

        public double CaloriesBurned { get; init; }

        [Smart]
        public DateTime WorkoutDate { get; init; }
    }
}
