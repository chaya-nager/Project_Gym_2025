using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class WorkoutVideoService: IService<WorkoutVideoDto>
    {
        private readonly IRepository<WorkoutVideo> repository;
        private readonly IMapper mapper;

        public WorkoutVideoService(IRepository<WorkoutVideo> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<WorkoutVideoDto> AddItemAsync(WorkoutVideoDto item)
        {
            return mapper.Map<WorkoutVideo, WorkoutVideoDto>(await repository.AddItemAsync(mapper.Map<WorkoutVideoDto, WorkoutVideo>(item)));
        }


        public async Task DeleteItemAsync(int id)
        {
            await repository.DeleteItemAsync(id);
        }

        public async Task<List<WorkoutVideoDto>> GetAllAsync()
        {
            return mapper.Map<List<WorkoutVideo>, List<WorkoutVideoDto>>(await repository.GetAllAsync());
        }

        public async Task<WorkoutVideoDto> GetByIdAsync(int id)
        {
            return mapper.Map<WorkoutVideo, WorkoutVideoDto>(await repository.GetByIdAsync(id));
        }

        public async Task UpdateItemAsync(int id, WorkoutVideoDto item)
        {
           await repository.UpdateItemAsync(id, mapper.Map<WorkoutVideoDto, WorkoutVideo>(item));
        }
    }
}
