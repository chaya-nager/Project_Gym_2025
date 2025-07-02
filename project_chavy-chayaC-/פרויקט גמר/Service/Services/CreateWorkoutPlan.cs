using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;

namespace Service.Services
{
    public class CreateWorkoutPlan
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<WorkoutVideo> videoRepository;
        private readonly IRepository<UserWorkoutPlan> planRepository;

        public CreateWorkoutPlan(
            IRepository<User> userRepository,
            IRepository<WorkoutVideo> videoRepository,
            IRepository<UserWorkoutPlan> planRepository)
        {
            this.userRepository = userRepository;
            this.videoRepository = videoRepository;
            this.planRepository = planRepository;
        }
        public async Task<List<WorkoutVideoDto>> GenerateWorkoutPlan(int userId,int desiredDuration,string difficultyLevel,string workoutType,string targetAudience,bool includeWarmup,bool includeCooldown)
        {
            Console.WriteLine("📥 נקלטה בקשה ל-GenerateWorkoutPlan");
            Console.WriteLine($"🧑‍ userId: {userId}, 💪 WorkoutType: {workoutType}, ⏱ Duration: {desiredDuration}, 🎯 Difficulty: {difficultyLevel}, 👥 Audience: {targetAudience}");
            Console.WriteLine($"🔥 Include Warmup: {includeWarmup}, ❄️ Include Cooldown: {includeCooldown}");

            var user = await userRepository.GetByIdAsync(userId);
            var allVideos = await videoRepository.GetAllAsync();

            Console.WriteLine($"🎞️ כמות סרטונים כולל בבסיס: {allVideos.Count}");

            var filteredVideos = allVideos.Where(v =>
                v.WorkoutType == workoutType &&
                v.Duration <= desiredDuration &&
                v.DifficultyLevel == difficultyLevel &&
                (string.IsNullOrEmpty(v.TargetAudience) || v.TargetAudience == targetAudience)
            ).ToList();

            var warmupVideos = allVideos
                .Where(v => v.Description.Contains("חימום", StringComparison.OrdinalIgnoreCase))
                .OrderBy(v => v.Duration)
                .ToList();

            var cooldownVideos = allVideos
                .Where(v => v.Description.Contains("מתיחות", StringComparison.OrdinalIgnoreCase))
                .OrderBy(v => v.Duration)
                .ToList();

            var mainVideos = filteredVideos
                .Except(warmupVideos)
                .Except(cooldownVideos)
                .OrderByDescending(v => v.Duration)
                .ToList();

            // שמור זמן לחימום ומתיחות מראש
            int warmupReserve = includeWarmup && warmupVideos.Any() ? warmupVideos.First().Duration : 0;
            int cooldownReserve = includeCooldown && cooldownVideos.Any() ? cooldownVideos.First().Duration : 0;
            int timeForMain = desiredDuration - warmupReserve - cooldownReserve;

            var candidate = new List<WorkoutVideo>();

            int remainingMain = timeForMain;

            foreach (var mv in mainVideos)
            {
                if (mv.Duration <= remainingMain)
                {
                    candidate.Add(mv);
                    remainingMain -= mv.Duration;
                    Console.WriteLine($"✅ נוסף אימון: {mv.Title}");
                }
            }

            if (includeWarmup && warmupReserve > 0)
            {
                var warmup = warmupVideos.FirstOrDefault(v => v.Duration == warmupReserve);
                if (warmup != null)
                {
                    candidate.Insert(0, warmup);
                    Console.WriteLine($"✅ נוסף חימום: {warmup.Title}");
                }
            }

            if (includeCooldown && cooldownReserve > 0)
            {
                var cooldown = cooldownVideos.FirstOrDefault(v => v.Duration == cooldownReserve);
                if (cooldown != null)
                {
                    candidate.Add(cooldown);
                    Console.WriteLine($"✅ נוספה מתיחה: {cooldown.Title}");
                }
            }

            // המרה ל־DTO
            var videoDtos = candidate.Select(v => new WorkoutVideoDto
            {
                VideoId = v.VideoId,
                Title = v.Title,
                Description = v.Description,
                Duration = v.Duration,
                DifficultyLevel = v.DifficultyLevel,
                WorkoutType = v.WorkoutType,
                TargetAudience = v.TargetAudience,
                VideoUrl = v.VideoUrl
            }).ToList();

            // שמירה במסד
            await planRepository.AddItemAsync(new UserWorkoutPlan
            {
                UserId = user.UserId,
                WorkoutPlanVideos = candidate
            });

            Console.WriteLine($"📦 כמות סופית: {candidate.Count}");
            return videoDtos;
        }



    }
}
