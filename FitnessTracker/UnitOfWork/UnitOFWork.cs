using FitnessTracker.GenericRepo;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.Repositories;

namespace FitnessTracker.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserContext _context;

        public IWorkoutRepository Workouts { get; private set; }
        public IGenericRepository<Meal> Meals { get; private set; }
        public IGoalRepository Goals { get; private set; } // changed to IGoalRepository
        public IUserRepository Users { get; private set; }

        public UnitOfWork(
            UserContext context,
            IUserRepository userRepository,
            IGoalRepository goalRepository // inject this
        )
        {
            _context = context;
            Workouts = new WorkoutRepository(_context);
            Meals = new GenericRepository<Meal>(_context);
            Goals = goalRepository;
            Users = userRepository;
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
