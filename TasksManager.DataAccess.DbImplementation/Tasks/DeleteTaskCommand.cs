using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TasksManager.DataAccess.Tasks;
using TasksManager.Db;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class DeleteTaskCommand : IDeleteTaskCommand
    {
        private TasksContext Context { get; }

        public DeleteTaskCommand(TasksContext context)
        {
            Context = context;
        }

        public async Task ExecuteAsync(int taskId)
        {
            Entities.Task task = await Context.Set<Entities.Task>()
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) throw new CannotDeleteTaskNotFound();

            foreach (var tagintask in task.Tags)
            {
                Context.TagsInTasks.Remove(tagintask);
            }
            Context.Set<Entities.Task>().Remove(task);

            await Context.SaveChangesAsync();
        }
    }
}
