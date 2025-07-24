using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.DTOs
{
    public record MealDTO(
        string Name,
        double? CaloriesPer100g,
        double? CaloriesPerPiece
    );
}
