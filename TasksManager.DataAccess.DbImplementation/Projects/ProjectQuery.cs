using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TasksManager.DataAccess.Projects;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    public class ProjectQuery : IProjectQuery
    {
        private TasksContext Context { get; }
        private IMapper Mapper { get; }

        public ProjectQuery(TasksContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<ProjectResponse> RunAsync(int projectId)
        {
            return await Context.Set<Project>()
                .Include(t => t.Tasks)
                .Select(p => Mapper.Map<Project, ProjectResponse>(p))
                .FirstOrDefaultAsync(t => t.Id == projectId);
        }
    }
}
