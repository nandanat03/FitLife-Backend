using AutoMapper;
using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services
{
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GoalService> _logger;

        public GoalService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GoalService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> AddGoalAsync(GoalDto dto)
        {
            if (dto.StartDate > dto.EndDate)
            {
                _logger.LogWarning("AddGoalAsync failed: StartDate {Start} is after EndDate {End} for user {UserId}", dto.StartDate, dto.EndDate, dto.UserId);
                return "InvalidDate";
            }

            var goal = _mapper.Map<Goal>(dto);
            await _unitOfWork.Goals.AddAsync(goal);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Goal added for user {UserId}: {GoalType} - {Value} from {Start} to {End}",
                dto.UserId, dto.GoalType, dto.GoalValue, dto.StartDate, dto.EndDate);

            return "Success";
        }

        public async Task<IEnumerable<GoalReportDto>> GetGoalReportAsync(int userId)
        {
            _logger.LogInformation("Fetching goal report for userId {UserId}", userId);

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

                return new GoalReportDto
                {
                    GoalId = goal.goalId,
                    UserId = goal.UserId,
                    GoalType = goal.GoalType.ToString(),
                    GoalValue = goal.GoalValue,
                    StartDate = goal.StartDate,
                    EndDate = goal.EndDate,
                    AchievedValue = achieved,
                    Remaining = Math.Max(goal.GoalValue - achieved, 0)
                };
            }); // Removed .ToList()

            _logger.LogInformation("Goal report generated for userId {UserId}", userId);
            return report;
        }


        public async Task<int?> DeleteGoalAsync(int goalId)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(goalId);
            if (goal == null)
            {
                _logger.LogWarning("DeleteGoalAsync failed: GoalId {GoalId} not found", goalId);
                return null;
            }

            await _unitOfWork.Goals.DeleteAsync(goal);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Goal deleted successfully. GoalId: {GoalId}", goalId);
            return goalId;
        }

        public async Task<(List<GoalDto> goals, int totalCount)> GetGoalsPaginatedAsync(int userId, int page, int pageSize)
        {
            var goalEntities = await _unitOfWork.Goals.GetPaginatedGoalsAsync(userId, page, pageSize);
            var totalCount = await _unitOfWork.Goals.GetTotalGoalCountAsync(userId);

            var goalDtos = _mapper.Map<List<GoalDto>>(goalEntities);

            return (goalDtos, totalCount);
        }

    }
}
