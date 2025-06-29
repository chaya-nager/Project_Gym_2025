using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock
{
    public class Database:DbContext,IContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserWorkoutPlan> UserWorkoutPlans { get; set; }
        public DbSet<WorkoutVideo> WorkoutVideos { get; set; }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=GymDB;trusted_connection=true;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // קשר User ל-UserWorkoutPlan (אחד לאחד)
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserWorkoutPlan)
                .WithOne(uwp => uwp.User)
                .HasForeignKey<UserWorkoutPlan>(uwp => uwp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // קשר UserWorkoutPlan ל-WorkoutVideo (אחד לרבים)
            modelBuilder.Entity<UserWorkoutPlan>()
                .HasMany(u => u.WorkoutPlanVideos)
                .WithOne(wv => wv.UserWorkoutPlan)
                 .HasForeignKey(wv => wv.UserWorkoutPlanId)
                 .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
