using System.Threading.Tasks;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.Tasks
{
    public interface IUpdateTaskCommand
    {
        Task<TaskResponse> ExecuteAsunc(int projectId, UpdateTaskRequest request);
    }
}
