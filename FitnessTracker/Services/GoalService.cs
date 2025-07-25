using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;

namespace FitnessTracker.Services
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _repo;

        public GoalService(IGoalRepository repo)
        {
            _repo = repo;
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

            await _repo.AddGoalAsync(goal);
            return "Success";
        }

        public async Task<List<object>> GetGoalReportAsync(int userId)
        {
            var goals = await _repo.GetGoalsByUserAsync(userId);
            var workouts = await _repo.GetWorkoutsByUserAsync(userId);

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
            var goal = await _repo.GetGoalByIdAsync(goalId);
            if (goal == null) return false;

            await _repo.DeleteGoalAsync(goal);
            return true;
        }
    }
}
