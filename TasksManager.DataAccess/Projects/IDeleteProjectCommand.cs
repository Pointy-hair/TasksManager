using System.Threading.Tasks;

namespace TasksManager.DataAccess.Projects
{
    public interface IDeleteProjectCommand
    {
        Task ExecuteAsync(int projectId);
    }
}
