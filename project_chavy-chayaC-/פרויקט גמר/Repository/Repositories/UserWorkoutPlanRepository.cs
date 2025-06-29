using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UserWorkoutPlanRepository : IRepository<UserWorkoutPlan>
    {
        private readonly IContext context;
        public UserWorkoutPlanRepository(IContext context)
        {
            this.context = context;
        }
        public async Task<UserWorkoutPlan> AddItemAsync(UserWorkoutPlan newPlan)
        {
            var existingPlan = await context.UserWorkoutPlans
                .Include(p => p.WorkoutPlanVideos)
                .FirstOrDefaultAsync(p => p.UserId == newPlan.UserId);

            if (existingPlan != null)
            {
                // אם קיים – נעדכן את הוידאוים
                existingPlan.WorkoutPlanVideos.Clear();
                foreach (var video in newPlan.WorkoutPlanVideos)
                {
                    existingPlan.WorkoutPlanVideos.Add(video);
                }
            }
            else
            {
                await context.UserWorkoutPlans.AddAsync(newPlan);         
            }
            await context.UserWorkoutPlans.AddAsync(newPlan);
            await context.SaveChangesAsync();
            return newPlan;
        }

        public async Task DeleteItemAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                context.UserWorkoutPlans.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<UserWorkoutPlan>> GetAllAsync()
        {
            return await context.UserWorkoutPlans.ToListAsync();
        }

        public async Task<UserWorkoutPlan> GetByIdAsync(int id)
        {
            return await context.UserWorkoutPlans.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateItemAsync(int id, UserWorkoutPlan item)
        {
            var userWorkoutPlan =await GetByIdAsync(id);
            if (userWorkoutPlan != null)
            {
                userWorkoutPlan.UserId = item.UserId;
                userWorkoutPlan.User = item.User;
                userWorkoutPlan.WorkoutPlanVideos = item.WorkoutPlanVideos;
                await context.SaveChangesAsync();
            }
        }
    }
}
