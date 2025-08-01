using FitnessTracker.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace FitnessTracker.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IProgressRepository _repo;
        private readonly ILogger<ProgressService> _logger;

        public ProgressService(IProgressRepository repo, ILogger<ProgressService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<object> GetDailySummaryAsync()
        {
            var today = DateTime.Today;
            _logger.LogInformation("Fetching daily summary for {Date}", today);

            var workouts = await _repo.GetWorkoutsByDateAsync(today);
            if (workouts == null || !workouts.Any())
            {
                _logger.LogWarning("No workouts found for {Date}", today);
            }

            string label = today.ToString("dd MMM");
            double calories = workouts.Sum(w => w.CaloriesBurned);
            float distance = workouts.Sum(w => w.Distance);
            float duration = workouts.Sum(w => w.Duration);

            _logger.LogInformation("Daily Summary - Calories: {Calories}, Distance: {Distance}, Duration: {Duration}", calories, distance, duration);

            return new
            {
                labels = new[] { label },
                data = new object[]
                {
                    new double[] { calories },
                    new float[] { distance },
                    new float[] { duration }
                }
            };
        }

        public async Task<object> GetMonthlySummaryAsync(int year)
        {
            _logger.LogInformation("Fetching monthly summary for year {Year}", year);

            var workouts = await _repo.GetWorkoutsByYearAsync(year);
            if (workouts == null || !workouts.Any())
            {
                _logger.LogWarning("No workouts found for year {Year}", year);
            }

            var grouped = workouts.GroupBy(w => w.WorkoutDate.Month).OrderBy(g => g.Key);

            var labels = new List<string>();
            var calories = new List<double>();
            var distance = new List<float>();
            var duration = new List<float>();

            foreach (var group in grouped)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(group.Key);
                labels.Add(monthName);
                calories.Add(group.Sum(w => w.CaloriesBurned));
                distance.Add(group.Sum(w => w.Distance));
                duration.Add(group.Sum(w => w.Duration));

                _logger.LogInformation("Month: {Month} | Calories: {Calories} | Distance: {Distance} | Duration: {Duration}",
                    monthName, calories.Last(), distance.Last(), duration.Last());
            }

            return new
            {
                labels,
                data = new object[]
                {
                    calories.ToArray(),
                    distance.ToArray(),
                    duration.ToArray()
                }
            };
        }

        public async Task<object> GetCustomSummaryAsync(DateTime start, DateTime end)
        {
            _logger.LogInformation("Fetching custom summary from {Start} to {End}", start, end);

            var workouts = await _repo.GetWorkoutsBetweenDatesAsync(start, end);
            if (workouts == null || !workouts.Any())
            {
                _logger.LogWarning("No workouts found between {Start} and {End}", start, end);
            }

            var grouped = workouts.GroupBy(w => w.WorkoutDate.Date).OrderBy(g => g.Key);

            var labels = new List<string>();
            var calories = new List<double>();
            var distance = new List<float>();
            var duration = new List<float>();

            foreach (var group in grouped)
            {
                string dateLabel = group.Key.ToString("dd MMM");
                labels.Add(dateLabel);
                calories.Add(group.Sum(w => w.CaloriesBurned));
                distance.Add(group.Sum(w => w.Distance));
                duration.Add(group.Sum(w => w.Duration));

                _logger.LogInformation("Date: {Date} | Calories: {Calories} | Distance: {Distance} | Duration: {Duration}",
                    dateLabel, calories.Last(), distance.Last(), duration.Last());
            }

            return new
            {
                labels,
                data = new object[]
                {
                    calories.ToArray(),
                    distance.ToArray(),
                    duration.ToArray()
                }
            };
        }
    }
}
