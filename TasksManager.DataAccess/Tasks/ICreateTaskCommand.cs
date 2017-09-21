using System.Threading.Tasks;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.Tasks
{
    public interface ICreateTaskCommand
    {
        Task<TaskResponse> ExecuteAsync(CreateTaskRequest request);
    }
}
