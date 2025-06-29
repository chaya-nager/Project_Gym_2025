using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Repository.Interfaces;
using Repository.Entities;
using AutoMapper;

namespace Service.Services
{
    public class UserService : IService<UserDto>
    {
        private readonly IRepository<User> repository;
        private readonly IMapper mapper;

        public UserService(IRepository<User> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<UserDto> AddItemAsync(UserDto item)
        {
            return mapper.Map<User, UserDto>(await repository.AddItemAsync(mapper.Map<UserDto, User>(item)));
        }

        public async Task DeleteItemAsync(int id)
        {
            await repository.DeleteItemAsync(id);
        }
        public async Task<List<UserDto>> GetAllAsync()
        {
            return mapper.Map <List<User>,List<UserDto>>(await repository.GetAllAsync());
        }
        public async Task<UserDto> GetByIdAsync(int id)
        {
           return mapper.Map<User, UserDto>(await repository.GetByIdAsync(id)); 
        }

        public async Task UpdateItemAsync(int id, UserDto item)
        {
            await repository.UpdateItemAsync(id, mapper.Map<UserDto, User>(item));
        }

    }
}
