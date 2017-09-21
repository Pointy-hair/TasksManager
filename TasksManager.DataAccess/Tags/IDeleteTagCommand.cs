using System.Threading.Tasks;

namespace TasksManager.DataAccess.Tags
{
    public interface IDeleteTagCommand
    {
        Task ExecuteAsync(int tagId);
    }
}
