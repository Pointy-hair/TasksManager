using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TasksManager.DataAccess.Projects;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    public class UpdateProjectCommand : IUpdateProjectCommand
    {
        private TasksContext Context { get; }
        private IMapper Mapper { get; }

        public UpdateProjectCommand(TasksContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<ProjectResponse> ExecuteAsunc(int projectId, UpdateProjectRequest request)
        {
            ProjectResponse response = await Context.Set<Project>()
                .Include(t => t.Tasks)
                .Select(p => Mapper.Map<Project, ProjectResponse>(p))
                .FirstOrDefaultAsync(pr => pr.Id == projectId);

            if (response == null) throw new CannotUpdateProjectNotFound();

            response = Mapper.Map(request, response);

            Context.Projects.Update(Mapper.Map<ProjectResponse, Project>(response));
            await Context.SaveChangesAsync();

            return response;
        }
    }
}
