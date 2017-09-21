using System.Threading.Tasks;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.Tasks
{
    public interface ITasksListQuery
    {
        Task<ListResponse<TaskResponse>> RunAsync(TaskFilter filter, ListOptions options);
    }
}
