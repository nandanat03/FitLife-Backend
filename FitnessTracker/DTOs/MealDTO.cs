using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.DTOs
{
    public class MealDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public double? CaloriesPer100g { get; set; }

        public double? CaloriesPerPiece { get; set; }
    }
}
