using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserWorkoutPlan> UserWorkoutPlans { get; set; }
        public DbSet<WorkoutVideo> WorkoutVideos { get; set; }
        public Task SaveChangesAsync();
    }
}
