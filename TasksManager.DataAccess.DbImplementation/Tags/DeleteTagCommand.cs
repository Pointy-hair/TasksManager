using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.Tags;
using TasksManager.Db;
using TasksManager.Entities;

namespace TasksManager.DataAccess.DbImplementation.Tags
{
    public class DeleteTagCommand : IDeleteTagCommand
    {
        private TasksContext Context { get; }
        private IMapper Mapper { get; }

        public DeleteTagCommand(TasksContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async System.Threading.Tasks.Task ExecuteAsync(int tagId)
        {
            Tag tag = await Context.Set<Tag>()
                .Include(t => t.Tasks)
                .ThenInclude(t => t.Task)
                .FirstOrDefaultAsync(t => t.Id == tagId);

            if (tag == null) throw new CannotDeleteTagNotFound();

            foreach (var tagsInTask in tag.Tasks)
            {
                Context.TagsInTasks.Remove(tagsInTask);
            }

            Context.Tags.Remove(tag);

            await Context.SaveChangesAsync();
        }
    }
}
