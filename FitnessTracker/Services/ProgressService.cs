using FitnessTracker.Interfaces;
using System.Globalization;

namespace FitnessTracker.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IProgressRepository _repo;

        public ProgressService(IProgressRepository repo)
        {
            _repo = repo;
        }

        public async Task<object> GetDailySummaryAsync()
        {
            var today = DateTime.Today;
            var workouts = await _repo.GetWorkoutsByDateAsync(today);

            string label = today.ToString("dd MMM");
            double calories = workouts.Sum(w => w.CaloriesBurned);
            float distance = workouts.Sum(w => w.Distance);
            float duration = workouts.Sum(w => w.Duration);

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
            var workouts = await _repo.GetWorkoutsByYearAsync(year);
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
            var workouts = await _repo.GetWorkoutsBetweenDatesAsync(start, end);
            var grouped = workouts.GroupBy(w => w.WorkoutDate.Date).OrderBy(g => g.Key);

            var labels = new List<string>();
            var calories = new List<double>();
            var distance = new List<float>();
            var duration = new List<float>();

            foreach (var group in grouped)
            {
                labels.Add(group.Key.ToString("dd MMM"));
                calories.Add(group.Sum(w => w.CaloriesBurned));
                distance.Add(group.Sum(w => w.Distance));
                duration.Add(group.Sum(w => w.Duration));
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
