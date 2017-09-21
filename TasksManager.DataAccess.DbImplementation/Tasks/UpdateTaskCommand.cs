using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksManager.DataAccess.Tasks;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class UpdateTaskCommand : IUpdateTaskCommand
    {
        private TasksContext Context { get; }
        private IMapper Mapper { get; }

        public UpdateTaskCommand(TasksContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<TaskResponse> ExecuteAsunc(int taskId, UpdateTaskRequest request)
        {
            Entities.Task task = await Context.Set<Entities.Task>()
                .Include(t => t.Tags)
                .ThenInclude(t => t.Tag)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) throw new CannotUpdateTaskNotFound();

            task = Mapper.Map(request, task);

            task.CompleteDate = request.Status == ViewModel.TaskStatus.Completed
                ? task.CompleteDate = DateTime.Now
                : task.CompleteDate = null;

            // Находим тэги, которые есть в таске, но нету в запросе
            // и удаляем связь между ними, т.к. в обновленной версии этих тэгов быть не должно
            var notFoundTags = task.Tags.Select(t => t.Tag.Name).Except(request.Tags).ToList();
            foreach (var tag in notFoundTags)
            {
                TagsInTask notTag = task.Tags.FirstOrDefault(t => t.Tag.Name == tag);

                task.Tags.Remove(notTag);
                Context.TagsInTasks.Remove(notTag);
            }

            // Находим тэги которые есть в запросе, но нету в таске
            // и связываем их между собой
            var notImplementedTags = request.Tags.Except(task.Tags.Select(t => t.Tag.Name)).ToList();
            foreach (var tagName in notImplementedTags)
            {
                var tag = await Context.Set<Tag>()
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

                task.Tags.Add(tagintask);
                tag.Tasks.Add(tagintask);

                await Context.TagsInTasks.AddAsync(tagintask);
            }

            Context.Tasks.Update(task);
            await Context.SaveChangesAsync();

            return Mapper.Map<Entities.Task, TaskResponse>(task);
        }
    }
}
