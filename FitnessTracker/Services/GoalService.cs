using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services
{
    public class GoalService : IGoalService
    {
        private readonly UserContext _context;

        public GoalService(UserContext context)
        {
            _context = context;
        }

        public async Task<string> AddGoalAsync(GoalDTO dto)
        {
            if (dto.StartDate > dto.EndDate)
                return "InvalidDate";

            var goal = new Goal
            {
                UserId = dto.UserId,
                GoalType = dto.GoalType,
                GoalValue = dto.GoalValue,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
            return "Success";
        }

        public async Task<List<object>> GetGoalReportAsync(int userId)
        {
            var goals = await _context.Goals
                .Where(g => g.UserId == userId)
                .ToListAsync();

            var workouts = await _context.Workouts
                .Where(w => w.UserId == userId)
                .ToListAsync();

            var report = goals.Select(goal =>
            {
                double achieved = 0;
                DateTime inclusiveEndDate = goal.EndDate.AddDays(1);

                var relevantWorkouts = workouts
                    .Where(w => w.WorkoutDate >= goal.StartDate && w.WorkoutDate < inclusiveEndDate);

                if (goal.GoalType == GoalType.Calories)
                {
                    achieved = relevantWorkouts.Sum(w => w.CaloriesBurned);
                }
                else if (goal.GoalType == GoalType.Distance)
                {
                    achieved = relevantWorkouts.Sum(w => w.Distance);
                }

                return new
                {
                    goalId = goal.goalId,
                    userId = goal.UserId,
                    goalType = goal.GoalType,
                    goalValue = goal.GoalValue,
                    startDate = goal.StartDate,
                    endDate = goal.EndDate,
                    achievedValue = achieved,
                    remaining = Math.Max(goal.GoalValue - achieved, 0)
                };
            }).ToList<object>();

            return report;
        }

        public async Task<bool> DeleteGoalAsync(int goalId)
        {
            var goal = await _context.Goals.FindAsync(goalId);
            if (goal == null) return false;

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
