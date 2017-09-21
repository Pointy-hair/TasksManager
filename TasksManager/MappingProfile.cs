using System;
using System.Collections.Generic;
using AutoMapper;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;
using System.Linq;
using TasksManager.ViewModel.Tags;
using TasksManager.ViewModel.Tasks;

namespace TasksManager
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectResponse>()
                .ForMember(dest => dest.OpenTasksCount,
                    opt => opt.MapFrom(src => src.Tasks.Count(t => t.Status != TaskStatus.Completed)))
                ;
            CreateMap<ProjectResponse, Project>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Tasks, opt => opt.Ignore())
                ;

            CreateMap<CreateProjectRequest, Project>();
            CreateMap<UpdateProjectRequest, ProjectResponse>();

            CreateMap<Tag, TagResponse>()
                .ForMember(dest => dest.OpenTasksCount,
                    opt => opt.MapFrom(src => src.Tasks.Count(t => t.Task.Status != TaskStatus.Completed)))
                .ForMember(dest => dest.TasksCount,
                    opt => opt.MapFrom(src => src.Tasks.Count))
                ;

            CreateMap<TagResponse, Tag>();

            CreateMap<CreateTaskRequest, Task>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => new List<TagsInTask>()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TaskStatus.Created))
                ;

            CreateMap<Task, TaskResponse>()
                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(str => str.Tags.Select(t => t.Tag.Name).ToArray()))
                ;

            CreateMap<UpdateTaskRequest, Task>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                ;

            CreateMap<Task, UpdateTaskRequest>()
                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(str => str.Tags.Select(t => t.Tag.Name).ToArray()))
                ;
        }
    }
}
