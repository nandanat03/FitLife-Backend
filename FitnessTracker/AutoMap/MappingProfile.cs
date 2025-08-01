using AutoMapper;
using FitnessTracker.Dtos;
using FitnessTracker.Models;

namespace FitnessTracker.AutoMap
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<Workout, WorkoutDto>().ReverseMap();

            
            CreateMap<Goal, GoalDto>()
                .ForMember(dest => dest.GoalType, opt => opt.MapFrom(src => src.GoalType))
                .ReverseMap();


            CreateMap<User, UserCreateDto>().ReverseMap();

           

        }
    }
}
