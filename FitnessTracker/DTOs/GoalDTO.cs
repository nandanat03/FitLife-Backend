using FitnessTracker.Models;
using System.ComponentModel.DataAnnotations;
using FitnessTracker.DTOs.SmartRequiredAttribute;

namespace FitnessTracker.Dtos
{
    public record GoalDto(
        int GoalId,
        [Smart] int UserId,
        [Smart] GoalType GoalType,
        [Smart] int GoalValue,
        [Smart] DateTime StartDate,
        [Smart] DateTime EndDate
    );
}
