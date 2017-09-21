using System.Threading.Tasks;
using AutoMapper;
using TasksManager.DataAccess.Projects;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModel.Projects;

namespace TasksManager.DataAccess.DbImplementation.Projects
{
    public class CreateProjectCommand : ICreateProjectCommand
    {
        private TasksContext Context { get; }
        private IMapper Mapper { get; }

        public CreateProjectCommand(TasksContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<ProjectResponse> ExecuteAsync(CreateProjectRequest request)
        {
            var project = Mapper.Map<CreateProjectRequest, Project>(request);

            await Context.Projects.AddAsync(project);
            await Context.SaveChangesAsync();

            var response = Mapper.Map<Project, ProjectResponse>(project);
            response.OpenTasksCount = 0;

            return response;
        }
    }
}
