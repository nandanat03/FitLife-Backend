using System.Globalization;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : Controller
    {


        private readonly UserContext _context;

        public ProgressController(UserContext context)
        {
            _context = context;
        }


        [HttpGet("daily")]
        public async Task<IActionResult> GetToday()
        {
            var today = DateTime.Today;

            // Fetch workouts from today into memory
            var workouts = await _context.Workouts
                .Where(w => w.WorkoutDate.Date == today)
                .ToListAsync(); // Avoids EF Core translation issues

            var label = today.ToString("dd MMM");

            // Summarize values manually
            double calories = workouts.Sum(w => w.CaloriesBurned);
            float distance = workouts.Sum(w => w.Distance);
            float duration = workouts.Sum(w => w.Duration);

            return Ok(new
            {
                labels = new[] { label },
                data = new object[]
                {
            new double[] { calories },
            new float[] { distance },
            new float[] { duration }
                }
            });
        }


        //  2. Monthly summary for given year
        [HttpGet("monthly/{year}")]
        public async Task<IActionResult> GetMonthly(int year)
        {
            // Fetch data from DB and bring to memory
            var workouts = await _context.Workouts
                .Where(w => w.WorkoutDate.Year == year)
                .ToListAsync();  // 🟢 This moves filtering to SQL, but GroupBy runs in C#

            // Now perform GroupBy in memory
            var grouped = workouts
                .GroupBy(w => w.WorkoutDate.Month)
                .OrderBy(g => g.Key);

            var labels = new List<string>();
            var calories = new List<double>();
            var distance = new List<float>();
            var duration = new List<float>();

            foreach (var group in grouped)
            {
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(group.Key);
                labels.Add(monthName);
                calories.Add(group.Sum(w => w.CaloriesBurned));
                distance.Add(group.Sum(w => w.Distance));
                duration.Add(group.Sum(w => w.Duration));
            }

            return Ok(new
            {
                labels,
                data = new object[]
                {
            calories.ToArray(),
            distance.ToArray(),
            duration.ToArray()
                }
            });
        }


        //  3. Custom date range
        [HttpGet("custom")]
        public async Task<IActionResult> GetCustom([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            // Load data into memory first
            var workouts = await _context.Workouts
                .Where(w => w.WorkoutDate.Date >= start.Date && w.WorkoutDate.Date <= end.Date)
                .ToListAsync();

            // Now group in memory
            var data = workouts
                .GroupBy(w => w.WorkoutDate.Date)
                .OrderBy(g => g.Key)
                .ToList();

            var labels = new List<string>();
            var calories = new List<double>();
            var distance = new List<float>();
            var duration = new List<float>();

            foreach (var group in data)
            {
                var label = group.Key.ToString("dd MMM");
                labels.Add(label);
                calories.Add(group.Sum(w => w.CaloriesBurned));
                distance.Add(group.Sum(w => w.Distance));
                duration.Add(group.Sum(w => w.Duration));
            }

            return Ok(new
            {
                labels,
                data = new object[]
                {
            calories.ToArray(),   // double[]
            distance.ToArray(),   // float[]
            duration.ToArray()    // float[]
                }
            });
        }

    }
}
