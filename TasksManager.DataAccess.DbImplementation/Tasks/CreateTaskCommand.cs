using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TasksManager.DataAccess.Tasks;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class CreateTaskCommand : ICreateTaskCommand
    {
        private TasksContext Context { get; }
        private IMapper Mapper { get; }

        public CreateTaskCommand(TasksContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<TaskResponse> ExecuteAsync(CreateTaskRequest request)
        {
            var project = await Context.Set<Project>()
                .FirstOrDefaultAsync(t => t.Id == request.ProjectId);

            if (project == null) throw new CannotCreateTaskProjectNotFound();

            Entities.Task task = Mapper.Map<CreateTaskRequest, Entities.Task>(request);

            foreach (var tagName in request.Tags)
            {
                Tag tag = await Context.Set<Tag>()
                              .Include(t => t.Tasks)
                              .FirstOrDefaultAsync(t => t.Name == tagName)
                          ?? new Tag
                          {
                              Name = tagName,
                              Tasks = new List<TagsInTask>()
                          };

                var tagintask = new TagsInTask
                {
                    Task = task,
                    Tag = tag
                };

                tag.Tasks.Add(tagintask);
                task.Tags.Add(tagintask);

                await Context.TagsInTasks.AddAsync(tagintask);
            }
            await Context.SaveChangesAsync();

            return Mapper.Map<Entities.Task, TaskResponse>(task);
        }
    }
}
