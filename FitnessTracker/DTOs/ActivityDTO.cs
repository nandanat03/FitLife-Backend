using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.DTOs
{
    public record ActivityDTO
    (
        [Required] string ActivityName,
        [Required] double MET_Value
    );
}
