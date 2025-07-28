using FitnessTracker.Models;
using FitnessTracker.GenericRepo;

namespace FitnessTracker.Interfaces
{
    public interface IGoalRepository : IGenericRepository<Goal>
    {
        // Add goal-specific methods here if needed in the future
    }
}
