using AutoMapper;
using FitnessTracker.DTOs;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.UnitOfWork;
using Serilog;

namespace FitnessTracker.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkoutService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddWorkoutAsync(WorkoutDTO dto)
        {
            Log.Information("Adding workout for userId {UserId} on {Date}", dto.UserId, dto.WorkoutDate);

            var workout = _mapper.Map<Workout>(dto);
            await _unitOfWork.Workouts.AddAsync(workout);
            var saved = await _unitOfWork.SaveAsync() > 0;

            if (saved)
                Log.Information("Workout added successfully for userId {UserId}", dto.UserId);
            else
                Log.Warning("Failed to add workout for userId {UserId}", dto.UserId);

            return saved;
        }

        public async Task<List<Workout>> GetUserWorkoutsAsync(int userId)
        {
            Log.Information("Fetching workouts for userId {UserId}", userId);

            var workouts = await _unitOfWork.Workouts.GetUserWorkoutsAsync(userId);

            Log.Information("{Count} workouts found for userId {UserId}", workouts.Count, userId);
            return workouts;
        }

        public async Task DeleteWorkoutAsync(int workoutId)
        {
            Log.Information("Attempting to delete workoutId {WorkoutId}", workoutId);

            var workout = await _unitOfWork.Workouts.GetByIdAsync(workoutId);
            if (workout != null)
            {
                await _unitOfWork.Workouts.DeleteAsync(workout);
                await _unitOfWork.SaveAsync();
                Log.Information("WorkoutId {WorkoutId} deleted", workoutId);
            }
            else
            {
                Log.Warning("WorkoutId {WorkoutId} not found for deletion", workoutId);
            }
        }

        public async Task<bool> DeleteMultipleAsync(List<int> workoutIds)
        {
            Log.Information("Deleting multiple workouts: {Ids}", string.Join(", ", workoutIds));

            var workouts = await _unitOfWork.Workouts.GetUserWorkoutsAsync(0); // Replace 0 with actual userId if needed
            var selected = workouts.Where(w => workoutIds.Contains(w.WorkoutId)).ToList();

            if (!selected.Any())
            {
                Log.Warning("No workouts found for deletion with provided IDs: {Ids}", string.Join(", ", workoutIds));
                return false;
            }

            var result = await _unitOfWork.Workouts.DeleteWorkoutsAsync(selected);
            await _unitOfWork.SaveAsync();
            Log.Information("{Count} workouts deleted", selected.Count);
            return result;
        }

        public async Task<List<object>> GetAllActivitiesAsync()
        {
            Log.Information("Fetching all activities");

            var activities = await _unitOfWork.Workouts.GetAllActivitiesAsync();

            Log.Information("{Count} activities fetched", activities.Count);
            return activities;
        }

        public async Task<string> AddActivityAsync(Activity newActivity)
        {
            Log.Information("Attempting to add activity: {ActivityName}", newActivity.ActivityName);

            if (await _unitOfWork.Workouts.ActivityExistsAsync(newActivity.ActivityName))
            {
                Log.Warning("Activity already exists: {ActivityName}", newActivity.ActivityName);
                return "Exists";
            }

            await _unitOfWork.Workouts.AddActivityAsync(newActivity);
            await _unitOfWork.SaveAsync();

            Log.Information("Activity added: {ActivityName}", newActivity.ActivityName);
            return "Success";
        }
    }
}
