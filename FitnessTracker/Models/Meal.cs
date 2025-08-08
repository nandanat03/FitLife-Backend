using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class Meal
    {
        [Key]
        public int MealId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? MealName { get; set; }

        public double? CaloriesPer100g { get; set; }

        public double? CaloriesPerPiece { get; set; }
    }
}
