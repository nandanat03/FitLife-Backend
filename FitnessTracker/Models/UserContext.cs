using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Models
{
    public class UserContext :DbContext
    {
        public UserContext(DbContextOptions options) : base(options){}

        public DbSet<User> Users { get; set; }

        public DbSet<Workout> Workouts { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<Goal> Goals { get; set; }

        public DbSet<Activity> Activities { get; set; }


    }
}
