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
    public class UserRepository : IRepository<User>
    {
        private readonly IContext context;
        public UserRepository(IContext context)
        {
            this.context = context;
        }
        public async Task<User> AddItemAsync(User item)
        {
            await context.Users.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteItemAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<User>>GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }
        public async Task UpdateItemAsync(int id, User item)
        {
            var user =await GetByIdAsync(id);
            if (user != null)
            {
                user.FullName = item.FullName;
                user.Email = item.Email;
                user.PasswordHash = item.PasswordHash;
                user.BirthDate = item.BirthDate;
                user.UserType = item.UserType;
                user.HealthConditions = item.HealthConditions;
                user.WorkoutVideos = item.WorkoutVideos;
                await context.SaveChangesAsync();
            }
        }
    }
}
