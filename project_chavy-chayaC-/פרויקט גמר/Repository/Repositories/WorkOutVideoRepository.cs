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
    public class WorkoutVideoRepository : IRepository<WorkoutVideo>
    {
        private readonly IContext context;
        public WorkoutVideoRepository(IContext context)
        {
            this.context = context;
        }
        public async Task<WorkoutVideo> AddItemAsync(WorkoutVideo item)
        {
            await context.WorkoutVideos.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteItemAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                context.WorkoutVideos.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<WorkoutVideo>> GetAllAsync()
        {
            return await context.WorkoutVideos.ToListAsync();
        }

        public async Task<WorkoutVideo> GetByIdAsync(int id)
        {
            return await context.WorkoutVideos.FirstOrDefaultAsync(x => x.VideoId == id);
        }

        public async Task UpdateItemAsync(int id, WorkoutVideo item)
        {
            var workoutVideo =await GetByIdAsync(id);
            if (workoutVideo != null)
            {
                workoutVideo.Title = item.Title;
                workoutVideo.Description = item.Description;
                workoutVideo.Duration = item.Duration;
                workoutVideo.DifficultyLevel = item.DifficultyLevel;
                workoutVideo.WorkoutType = item.WorkoutType;
                workoutVideo.TargetAudience = item.TargetAudience;
                workoutVideo.VideoUrl = item.VideoUrl;
                workoutVideo.UploadedAt = item.UploadedAt;
                workoutVideo.TrainerId = item.TrainerId;
                workoutVideo.Trainer = item.Trainer;
                await context.SaveChangesAsync();
            }
        }
    }
}
