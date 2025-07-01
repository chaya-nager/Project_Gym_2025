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
        //public async Task<UserWorkoutPlanDto> AddItemAsync(UserWorkoutPlanDto item)
        //{
        //    return mapper.Map<UserWorkoutPlan, UserWorkoutPlanDto>(await  repository.AddItemAsync(mapper.Map<UserWorkoutPlanDto,UserWorkoutPlan>(item)));
        //}
        public async Task<UserWorkoutPlanDto> AddItemAsync(UserWorkoutPlanDto item)
        {
            var entity = await ConvertDtoToEntity(item);
            var added = await repository.AddItemAsync(entity);
            return mapper.Map<UserWorkoutPlanDto>(added);
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
        public async Task<UserWorkoutPlan> ConvertDtoToEntity(UserWorkoutPlanDto dto)
        {
            var videos = await videoRepository.GetAllAsync(); // או GetByIds
            var selectedVideos = videos.Where(v => dto.VideoIds.Contains(v.VideoId)).ToList();

            var entity = mapper.Map<UserWorkoutPlan>(dto);
            entity.WorkoutPlanVideos = selectedVideos;

            return entity;
        }
    }
}
