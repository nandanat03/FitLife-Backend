using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitnessTracker.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public string? ActivityType { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int Distance { get; set; }

        public double CaloriesBurned { get; set; }

        [Required]
        public DateTime WorkoutDate { get; set; }
    }
}
