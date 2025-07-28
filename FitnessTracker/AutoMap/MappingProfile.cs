using AutoMapper;
using FitnessTracker.DTOs;
using FitnessTracker.Models;

namespace FitnessTracker.AutoMap
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<Workout, WorkoutDTO>().ReverseMap();

            
            CreateMap<Goal, GoalDTO>()
                .ForMember(dest => dest.GoalType, opt => opt.MapFrom(src => src.GoalType))
                .ReverseMap();


            CreateMap<User, UserCreateDto>().ReverseMap();

           

        }
    }
}
