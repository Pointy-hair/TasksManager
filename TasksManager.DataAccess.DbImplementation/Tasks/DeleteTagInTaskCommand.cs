using Microsoft.EntityFrameworkCore;
using System.Linq;
using TasksManager.DataAccess.Tasks;
using TasksManager.Db;
using TasksManager.Entities;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class DeleteTagInTaskCommand : IDeleteTagInTaskCommand
    {
        private TasksContext Context { get; }

        public DeleteTagInTaskCommand(TasksContext context)
        {
            Context = context;
        }

        public async System.Threading.Tasks.Task ExecuteAsync(int taskId, int tagId)
        {
            Task task = await Context.Set<Task>()
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) throw new CannotDeleteTaskNotFound();

            TagsInTask tagintask = task.Tags.FirstOrDefault(t => t.TagId == tagId);

            if (tagintask == null) throw new CannotDeleteTagInTaskNotFound();

            Context.TagsInTasks.Remove(tagintask);
            await Context.SaveChangesAsync();
        }
    }
}
