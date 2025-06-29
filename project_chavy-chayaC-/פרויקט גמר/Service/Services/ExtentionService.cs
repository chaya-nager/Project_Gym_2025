using Common.Dto;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public static class ExtentionService
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddRepository();
            services.AddScoped<IService<UserDto>, UserService>();
            services.AddScoped<IService<UserWorkoutPlanDto>, UserWorkoutPlanService>();
            services.AddScoped<IService<WorkoutVideoDto>, WorkoutVideoService>();
            services.AddAutoMapper(typeof(MyMapper));
            return services;
        }
    }
}
