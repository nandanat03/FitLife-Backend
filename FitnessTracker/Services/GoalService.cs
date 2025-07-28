using AutoMapper;
using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using Serilog;
using FitnessTracker.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services
{
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GoalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string> AddGoalAsync(GoalDTO dto)
        {
            if (dto.StartDate > dto.EndDate)
            {
                Log.Warning("AddGoalAsync failed: StartDate {Start} is after EndDate {End} for user {UserId}", dto.StartDate, dto.EndDate, dto.UserId);
                return "InvalidDate";
            }

            var goal = _mapper.Map<Goal>(dto);
            await _unitOfWork.Goals.AddAsync(goal);
            await _unitOfWork.SaveAsync();

            Log.Information("Goal added for user {UserId}: {GoalType} - {Value} from {Start} to {End}",
                dto.UserId, dto.GoalType, dto.GoalValue, dto.StartDate, dto.EndDate);

            return "Success";
        }

        public async Task<List<object>> GetGoalReportAsync(int userId)
        {
            Log.Information("Fetching goal report for userId {UserId}", userId);

            var goals = await _unitOfWork.Goals.Where(g => g.UserId == userId).ToListAsync();
            var workouts = await _unitOfWork.Workouts.Where(w => w.UserId == userId).ToListAsync();

            var report = goals.Select(goal =>
            {
                double achieved = 0;
                DateTime inclusiveEndDate = goal.EndDate.AddDays(1);

                var relevantWorkouts = workouts
                    .Where(w => w.WorkoutDate >= goal.StartDate && w.WorkoutDate < inclusiveEndDate);

                if (goal.GoalType == GoalType.Calories)
                    achieved = relevantWorkouts.Sum(w => w.CaloriesBurned);
                else if (goal.GoalType == GoalType.Distance)
                    achieved = relevantWorkouts.Sum(w => w.Distance);

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

            Log.Information("Goal report generated for userId {UserId}", userId);
            return report;
        }

        public async Task<bool> DeleteGoalAsync(int goalId)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(goalId);
            if (goal == null)
            {
                Log.Warning("DeleteGoalAsync failed: GoalId {GoalId} not found", goalId);
                return false;
            }

            await _unitOfWork.Goals.DeleteAsync(goal);
            await _unitOfWork.SaveAsync();

            Log.Information("Goal deleted successfully. GoalId: {GoalId}", goalId);
            return true;
        }
    }

}
