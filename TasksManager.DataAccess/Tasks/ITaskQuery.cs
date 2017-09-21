using System.Threading.Tasks;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.Tasks
{
    public interface ITaskQuery
    {
        Task<TaskResponse> RunAsync(int taskId);
    }
}
