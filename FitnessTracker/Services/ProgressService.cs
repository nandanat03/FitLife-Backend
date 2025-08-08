using FitnessTracker.Dtos;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.UnitOfWork;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace FitnessTracker.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProgressService> _logger;

        public ProgressService(IUnitOfWork unitOfWork, ILogger<ProgressService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ProgressDto> GetDailySummaryAsync(int userId)
        {
            var today = DateTime.Today;
            _logger.LogInformation("Fetching daily summary for {Date}", today);

            var workouts = await _unitOfWork.Progress.GetWorkoutsByDateAsync(today, userId)
                           ?? Enumerable.Empty<Workout>();

            return BuildProgressDto(today.ToString("dd MMM"), workouts);
        }

        public async Task<ProgressDto> GetMonthlySummaryAsync(int year, int userId)
        {
            _logger.LogInformation("Fetching monthly summary for year {Year}", year);

            var workouts = await _unitOfWork.Progress.GetWorkoutsByYearAsync(year, userId)
                           ?? Enumerable.Empty<Workout>();

            if (!workouts.Any())
                return BuildProgressDto(Enumerable.Empty<string>(), Enumerable.Empty<double>(), Enumerable.Empty<float>(), Enumerable.Empty<float>());

            var grouped = workouts
                .GroupBy(w => w.WorkoutDate.Month)
                .OrderBy(g => g.Key);

            var labels = new List<string>();
            var calories = new List<double>();
            var distances = new List<float>();
            var durations = new List<float>();

            foreach (var group in grouped)
            {
                labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(group.Key));
                calories.Add(group.Sum(w => w.CaloriesBurned));
                distances.Add(group.Sum(w => w.Distance));
                durations.Add(group.Sum(w => w.Duration));
            }

            return BuildProgressDto(labels, calories, distances, durations);
        }

        public async Task<ProgressDto> GetCustomSummaryAsync(DateTime start, DateTime end, int userId)
        {
            _logger.LogInformation("Fetching custom summary from {Start} to {End}", start, end);

            var workouts = await _unitOfWork.Progress.GetWorkoutsBetweenDatesAsync(start, end, userId)
                           ?? Enumerable.Empty<Workout>();

            if (!workouts.Any())
                return BuildProgressDto(Enumerable.Empty<string>(), Enumerable.Empty<double>(), Enumerable.Empty<float>(), Enumerable.Empty<float>());

            var grouped = workouts
                .GroupBy(w => w.WorkoutDate.Date)
                .OrderBy(g => g.Key);

            var labels = new List<string>();
            var calories = new List<double>();
            var distances = new List<float>();
            var durations = new List<float>();

            foreach (var group in grouped)
            {
                labels.Add(group.Key.ToString("dd MMM"));
                calories.Add(group.Sum(w => w.CaloriesBurned));
                distances.Add(group.Sum(w => w.Distance));
                durations.Add(group.Sum(w => w.Duration));
            }

            return BuildProgressDto(labels, calories, distances, durations);
        }


        private ProgressDto BuildProgressDto(string label, IEnumerable<Workout> workouts)
        {
            double calories = workouts.Sum(w => w.CaloriesBurned);
            float distance = workouts.Sum(w => w.Distance);
            float duration = workouts.Sum(w => w.Duration);

            return new ProgressDto(
                Labels: new[] { label },
                Data: new object[]
                {
                    new double[] { calories },
                    new float[] { distance },
                    new float[] { duration }
                }
            );
        }

        private ProgressDto BuildProgressDto(IEnumerable<string> labels, IEnumerable<double> calories, IEnumerable<float> distances, IEnumerable<float> durations)
        {
            return new ProgressDto(
                Labels: labels.ToArray(),
                Data: new object[]
                {
                    calories.ToArray(),
                    distances.ToArray(),
                    durations.ToArray()
                }
            );
        }
    }
}
