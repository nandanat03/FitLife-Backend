using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using FitnessTracker.GenericRepo;

namespace FitnessTracker.Repositories
{
    public class GoalRepository : GenericRepository<Goal>, IGoalRepository
    {
        public GoalRepository(UserContext context) : base(context)
        {
        }
    }
}
