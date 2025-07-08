using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FitnessTracker.Models
{
    public enum GoalType
    {
     
        Distance,
        Calories
    }
    public class Goal
    {
        [Key]

        public int goalId { get; set; }

        [Required]
        public int UserId { get; set; }

        [JsonIgnore]
        [ValidateNever]
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public GoalType GoalType { get; set; }

        [Required]
        public int GoalValue { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }


    }
}
