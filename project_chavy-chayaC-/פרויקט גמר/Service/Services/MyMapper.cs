using AutoMapper;
using Common.Dto;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class MyMapper:Profile
    {
        string path=Path.Combine(Environment.CurrentDirectory, "Videos//");

        public MyMapper()
        {
            CreateMap<WorkoutVideo, WorkoutVideoDto>()
                .ForMember(dest => dest.VideoArr, opt => opt.Ignore());  // המיפוי לא טוען את הקובץ
            CreateMap<WorkoutVideoDto, WorkoutVideo>()
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.fileVideo.FileName));
          //  CreateMap<UserWorkoutPlan, UserWorkoutPlanDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserWorkoutPlan, UserWorkoutPlanDto>()
    .ForMember(dest => dest.VideoIds, opt => opt.MapFrom(src => src.WorkoutPlanVideos.Select(v => v.VideoId)));

            CreateMap<UserWorkoutPlanDto, UserWorkoutPlan>()
                .ForMember(dest => dest.WorkoutPlanVideos, opt => opt.Ignore());

        }
    }
}
