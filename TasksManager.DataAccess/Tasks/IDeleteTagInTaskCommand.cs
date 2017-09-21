using System.Threading.Tasks;

namespace TasksManager.DataAccess.Tasks
{
    public interface IDeleteTagInTaskCommand
    {
        Task ExecuteAsync(int taskId, int tagId);
    }
}
