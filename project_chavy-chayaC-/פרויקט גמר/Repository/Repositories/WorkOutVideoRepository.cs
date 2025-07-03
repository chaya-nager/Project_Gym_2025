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
            var video = await context.WorkoutVideos.FindAsync(id);
            if (video == null)
                throw new Exception("Video not found");

            // מחיקת הקובץ מהשרת
            if (!string.IsNullOrEmpty(video.VideoUrl))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos", video.VideoUrl);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            // מחיקת הרשומה מה-DB
            context.WorkoutVideos.Remove(video);
            await context.SaveChangesAsync();
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
            var workoutVideo = await GetByIdAsync(id);
            if (workoutVideo != null)
            {
                if (!string.IsNullOrWhiteSpace(item.Title) && workoutVideo.Title != item.Title)
                    workoutVideo.Title = item.Title;

                if (!string.IsNullOrWhiteSpace(item.Description) && workoutVideo.Description != item.Description)
                    workoutVideo.Description = item.Description;

                if (item.Duration > 0 && workoutVideo.Duration != item.Duration)
                    workoutVideo.Duration = item.Duration;

                if (!string.IsNullOrWhiteSpace(item.DifficultyLevel) && workoutVideo.DifficultyLevel != item.DifficultyLevel)
                    workoutVideo.DifficultyLevel = item.DifficultyLevel;

                if (!string.IsNullOrWhiteSpace(item.WorkoutType) && workoutVideo.WorkoutType != item.WorkoutType)
                    workoutVideo.WorkoutType = item.WorkoutType;

                if (!string.IsNullOrWhiteSpace(item.TargetAudience) && workoutVideo.TargetAudience != item.TargetAudience)
                    workoutVideo.TargetAudience = item.TargetAudience;
                if(!string.IsNullOrWhiteSpace(item.TargetAgeGroup) && workoutVideo.TargetAgeGroup != item.TargetAgeGroup)
                    workoutVideo.TargetAgeGroup = item.TargetAgeGroup;
                if (!string.IsNullOrWhiteSpace(item.VideoUrl) && workoutVideo.VideoUrl != item.VideoUrl)
                    workoutVideo.VideoUrl = item.VideoUrl;

                if (item.UploadedAt != default && workoutVideo.UploadedAt != item.UploadedAt)
                    workoutVideo.UploadedAt = item.UploadedAt;

                if (item.TrainerId > 0 && workoutVideo.TrainerId != item.TrainerId)
                {
                    workoutVideo.TrainerId = item.TrainerId;
                    workoutVideo.Trainer = item.Trainer;
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
