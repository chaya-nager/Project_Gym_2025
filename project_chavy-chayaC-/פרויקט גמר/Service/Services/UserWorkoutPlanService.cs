using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
namespace Service.Services
{
    public class UserWorkoutPlanService : IService<UserWorkoutPlanDto>
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<WorkoutVideo> videoRepository;
        private readonly IRepository<UserWorkoutPlan> repository;
        private readonly IMapper mapper;
        public UserWorkoutPlanService(IRepository<User> userRepository, IRepository<WorkoutVideo> videoRepository,IRepository<UserWorkoutPlan> repository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.videoRepository = videoRepository;
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<UserWorkoutPlanDto> AddItemAsync(UserWorkoutPlanDto item)
        {
            return mapper.Map<UserWorkoutPlan, UserWorkoutPlanDto>(await  repository.AddItemAsync(mapper.Map<UserWorkoutPlanDto,UserWorkoutPlan>(item)));
        }

        public async Task DeleteItemAsync(int id)
        {
            await repository.DeleteItemAsync(id);
        }

        public async Task<List<UserWorkoutPlanDto>> GetAllAsync()
        { 
            return mapper.Map<List<UserWorkoutPlan>, List<UserWorkoutPlanDto>>(await repository.GetAllAsync());
        }
        public async Task<UserWorkoutPlanDto> GetByIdAsync(int id)
        {
            return mapper.Map<UserWorkoutPlan, UserWorkoutPlanDto>(await repository.GetByIdAsync(id));
        }

        public async Task UpdateItemAsync(int id, UserWorkoutPlanDto item)
        {
             await repository.UpdateItemAsync(id, mapper.Map<UserWorkoutPlanDto, UserWorkoutPlan>(item));
        }
        //הלוגיקה מזיפוש
        //public UserWorkoutPlan GenerateWorkoutPlan(int userId, int desiredDuration, string difficultyLevel)
        //{
        //    var user = userRepository.GetByIdAsync(userId);
        //    var allVideos = videoRepository.GetAllAsync();

        //    var filtered = allVideos
        //        .Where(v =>
        //            v.DifficultyLevel == difficultyLevel &&
        //            (string.IsNullOrEmpty(v.TargetAudience) ||
        //             !user.HealthConditions?.Split(',').Any(cond => v.TargetAudience.Contains(cond.Trim())) == true) &&
        //            v.Duration <= desiredDuration &&
        //            (user.BirthDate.AddYears(13) <= DateTime.Today || v.TargetAudience.Contains("ילדים")))
        //        .ToList();

        //    var scoredVideos = filtered
        //        .Select(v => new
        //        {
        //            Video = v,
        //            Score = CalculateScore(v, user, desiredDuration, difficultyLevel)
        //        })
        //        .OrderByDescending(x => x.Score)
        //        .ToList();

        //    var selected = new List<WorkoutVideo>();
        //    int totalTime = 0;

        //    foreach (var item in scoredVideos)
        //    {
        //        if (totalTime + item.Video.Duration <= desiredDuration)
        //        {
        //            selected.Add(item.Video);
        //            totalTime += item.Video.Duration;
        //        }
        //    }

        //    var plan = new UserWorkoutPlan
        //    {
        //        UserId = user.Id,
        //        WorkoutPlanVideos = selected
        //    };

        //    repository.AddItemAsync(plan);
        //    return plan;
        //}

        //private double CalculateScore(WorkoutVideo video, User user, int desiredDuration, string difficultyLevel)
        //{
        //    double score = 0;

        //    score += 1 - (Math.Abs(video.Duration - desiredDuration) / (double)desiredDuration);

        //    if (video.DifficultyLevel == difficultyLevel)
        //        score += 1;
        //    else
        //        score += 0.5;

        //    if (!string.IsNullOrEmpty(user.HealthConditions) &&
        //        !string.IsNullOrEmpty(video.TargetAudience) &&
        //        user.HealthConditions.Split(',').Any(cond => video.TargetAudience.Contains(cond.Trim())))
        //        score -= 1;

        //    return score;
        //}
    }
}
