﻿using FitnessTracker.Models;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.DTOs
{
    public record GoalDTO
    (
        [Required] int UserId,
        [Required] GoalType GoalType,
        [Required] int GoalValue,
        [Required] DateTime StartDate,
        [Required] DateTime EndDate
    );
}
