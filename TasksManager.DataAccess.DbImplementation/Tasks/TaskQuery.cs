using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TasksManager.DataAccess.Tasks;
using TasksManager.Db;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class TaskQuery : ITaskQuery
    {
        private TasksContext Context { get; }
        private IMapper Mapper { get; }

        public TaskQuery(TasksContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<TaskResponse> RunAsync(int taskId)
        {
            return await Context.Set<Entities.Task>()
                .Include(t => t.Tags)
                .ThenInclude(t => t.Tag)
                .Include(p => p.Project)
                .Select(task => Mapper.Map<Entities.Task, TaskResponse>(task))
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }
    }
}
