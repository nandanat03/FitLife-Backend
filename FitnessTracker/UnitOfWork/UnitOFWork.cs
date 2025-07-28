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
        public IGenericRepository<Goal> Goals { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(
            UserContext context,
            IUserRepository userRepository 
        )
        {
            _context = context;
            Workouts = new WorkoutRepository(_context);
            Meals = new GenericRepository<Meal>(_context);
            Goals = new GenericRepository<Goal>(_context);
            Users = userRepository; 
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }

}
