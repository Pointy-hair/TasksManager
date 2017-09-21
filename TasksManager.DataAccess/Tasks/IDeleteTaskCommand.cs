using System.Threading.Tasks;

namespace TasksManager.DataAccess.Tasks
{
    public interface IDeleteTaskCommand
    {
        Task ExecuteAsync(int taskId);
    }
}
