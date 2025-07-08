using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FitnessTracker.Models
{
    public class Workout
    {
        [Key]

        public int WorkoutId { get; set; }

        [Required]
        public int UserId { get; set; }

        [JsonIgnore]
        [ValidateNever]
        [ForeignKey("UserId")]
        public User User { get; set; }

        public string ActivityType { get; set; }
        public int Duration { get; set; }
        public int Distance {  get; set; }
        public double CaloriesBurned { get; set; }
        public DateTime WorkoutDate { get; set; }


    }
}
