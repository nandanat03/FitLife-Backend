using FitnessTracker.GenericRepo;
using FitnessTracker.Interfaces;
using FitnessTracker.Models;
using System;

namespace FitnessTracker.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IWorkoutRepository Workouts { get; }
        IGenericRepository<Meal> Meals { get; }
        IGoalRepository Goals { get; } // changed from IGenericRepository<Goal>
        IUserRepository Users { get; }
        Task<int> SaveAsync();
    }
}
