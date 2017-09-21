using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.DataAccess.Projects;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    public class DeleteProjectCommand : IDeleteProjectCommand
    {
        private TasksContext Context { get; }
        private IMapper Mapper { get; }

        public DeleteProjectCommand(TasksContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async System.Threading.Tasks.Task ExecuteAsync(int projectId)
        {
            var project = await Context.Set<Project>()
                .Include(t => t.Tasks)
                .Select(p => Mapper.Map<Project, ProjectResponse>(p))
                .FirstOrDefaultAsync(t => t.Id == projectId);

            if (project == null) throw new CannotDeleteProjectNotFound();
            if (project.OpenTasksCount > 0) throw new CannotDeleteProjectWithTasksException();

            Context.Set<Project>().Remove(Mapper.Map<ProjectResponse, Project>(project));
            await Context.SaveChangesAsync();
        }
    }
}
