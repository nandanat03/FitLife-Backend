namespace FitnessTracker.Models
{
    public class Meal
    {
        public int MealId { get; set; }
        public string MealName { get; set; }
        public double? CaloriesPer100g { get; set; }
        public double? CaloriesPerPiece { get; set; }
    }
}
