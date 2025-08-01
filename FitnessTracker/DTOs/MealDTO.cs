using System.ComponentModel.DataAnnotations;
using FitnessTracker.DTOs.SmartRequiredAttribute;

namespace FitnessTracker.Dtos
{
    public class MealDto
    {
        public int MealId { get; set; }
        [Smart]
        public string Name { get; set; } = string.Empty;

        public double? CaloriesPer100g { get; set; }

        public double? CaloriesPerPiece { get; set; }
    }
}
