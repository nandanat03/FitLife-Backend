using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FitnessTracker.DTOs
{
    public record ActivityDTO
    {
        [Required]
        [JsonPropertyName("activityName")]
        public string ActivityName { get; set; }

        [Required]
        [JsonPropertyName("met_Value")]
        public double MET_Value { get; set; }
    }
}
