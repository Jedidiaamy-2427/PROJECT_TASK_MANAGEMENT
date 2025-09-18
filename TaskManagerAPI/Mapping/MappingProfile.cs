using AutoMapper;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<TaskItem, TaskItemDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}


