using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FitnessTracker.DTOs.SmartRequiredAttribute;

namespace FitnessTracker.Dtos
{
    public record ActivityDto
    {
        [Smart]
        [JsonPropertyName("activityName")]
        public string ActivityName { get; set; }

        [Smart]
        [JsonPropertyName("met_Value")]
        public double MET_Value { get; set; }
    }
}
