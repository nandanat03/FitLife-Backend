using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        public string? ActivityName { get; set; }

        [Required]
        public double MET_Value { get; set; }
    }
}
