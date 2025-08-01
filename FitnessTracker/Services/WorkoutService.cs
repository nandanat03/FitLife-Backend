using AutoMapper;
using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<WorkoutService> _logger;

        public WorkoutService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<WorkoutService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AddWorkoutAsync(WorkoutDto dto)
        {
            _logger.LogInformation("Adding workout for userId {UserId} on {Date}", dto.UserId, dto.WorkoutDate);

            var workout = _mapper.Map<Workout>(dto);
            await _unitOfWork.Workouts.AddAsync(workout);
            var saved = await _unitOfWork.SaveAsync() > 0;

            if (saved)
                _logger.LogInformation("Workout added successfully for userId {UserId}", dto.UserId);
            else
                _logger.LogWarning("Failed to add workout for userId {UserId}", dto.UserId);

            return saved;
        }

        public async Task<List<Workout>> GetUserWorkoutsAsync(int userId)
        {
            _logger.LogInformation("Fetching workouts for userId {UserId}", userId);

            var workouts = await _unitOfWork.Workouts.GetUserWorkoutsAsync(userId);

            _logger.LogInformation("{Count} workouts found for userId {UserId}", workouts.Count, userId);
            return workouts;
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            _logger.LogInformation("Attempting to delete workoutId {WorkoutId}", workoutId);

            var workout = await _unitOfWork.Workouts.GetByIdAsync(workoutId);
            if (workout != null)
            {
                await _unitOfWork.Workouts.DeleteAsync(workout);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("WorkoutId {WorkoutId} deleted", workoutId);
            }
            else
            {
                _logger.LogWarning("WorkoutId {WorkoutId} not found for deletion", workoutId);
            }
        }

        public async Task<bool> DeleteMultipleAsync(List<int> workoutIds)
        {
            _logger.LogInformation("Deleting multiple workouts: {Ids}", string.Join(", ", workoutIds));

            var workouts = await _unitOfWork.Workouts.GetUserWorkoutsAsync(0); // Adjust userId if needed
            var selected = workouts.Where(w => workoutIds.Contains(w.WorkoutId)).ToList();

            if (!selected.Any())
            {
                _logger.LogWarning("No workouts found for deletion with provided IDs: {Ids}", string.Join(", ", workoutIds));
                return false;
            }

            var result = await _unitOfWork.Workouts.DeleteWorkoutsAsync(selected);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("{Count} workouts deleted", selected.Count);
            return result;
        }

        public async Task<List<ActivityDto>> GetAllActivitiesAsync()
        {
            _logger.LogInformation("Fetching all activities from database");

            var activityEntities = await _unitOfWork.Workouts.GetAllActivitiesAsync(); // From repository

            var activityDtos = activityEntities.Select(a => new ActivityDto
            {
                ActivityName = a.ActivityName,
                MET_Value = a.MET_Value
            }).ToList();

            _logger.LogInformation("{Count} activities fetched successfully", activityDtos.Count);
            return activityDtos;
        }


        public async Task<string> AddActivityAsync(ActivityDto newActivity)
        {
            _logger.LogInformation("Attempting to add activity: {ActivityName}", newActivity.ActivityName);

            if (await _unitOfWork.Workouts.ActivityExistsAsync(newActivity.ActivityName))
            {
                _logger.LogWarning("Activity already exists: {ActivityName}", newActivity.ActivityName);
                return "Exists";
            }

            var activity = new Activity
            {
                ActivityName = newActivity.ActivityName,
                MET_Value = newActivity.MET_Value
            };

            await _unitOfWork.Workouts.AddActivityAsync(activity);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Activity added: {ActivityName}", newActivity.ActivityName);
            return "Success";
        }
    }
}
