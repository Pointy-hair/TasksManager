using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TasksManager.DataAccess.Tasks;
using TasksManager.Db;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.DataAccess.DbImplementation.Tasks
{
    public class TasksListQuery : ITasksListQuery
    {
        private TasksContext Context { get; }
        private IMapper Mapper { get; }

        public TasksListQuery(TasksContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<ListResponse<TaskResponse>> RunAsync(TaskFilter filter, ListOptions options)
        {
            IQueryable<TaskResponse> query = Context.Set<Entities.Task>()
                .Include(t => t.Tags)
                .ThenInclude(t => t.Tag)
                .Include(p => p.Project)
                .Select(task => Mapper.Map<Entities.Task, TaskResponse>(task));

            query = ApplyFilter(query, filter);

            int totalCount = await query.CountAsync();
            if (options.Sort == null)
            {
                options.Sort = "Id";
            }

            query = options.ApplySort(query);
            query = options.ApplyPaging(query);

            return new ListResponse<TaskResponse>
            {
                Items = await query.ToListAsync(),
                Page = options.Page,
                PageSize = options.PageSize ?? totalCount,
                Sort = options.Sort,
                TotalItemsCount = totalCount
            };
        }

        private IQueryable<TaskResponse> ApplyFilter(IQueryable<TaskResponse> query, TaskFilter filter)
        {
            if (filter.Id != null)
            {
                query = query.Where(pr => pr.Id == filter.Id);
            }

            if (filter.Name != null)
            {
                query = query.Where(pr => pr.Name.StartsWith(filter.Name));
            }

            if (filter.CreateDate != null)
            {
                if (filter.CreateDate.From != null)
                {
                    query = query.Where(t => t.CreateDate >= filter.CreateDate.From);
                }

                if (filter.CreateDate.To != null)
                {
                    query = query.Where(t => t.CreateDate <= filter.CreateDate.To);
                }
            }

            if (filter.CompleteDate != null)
            {
                if (filter.CompleteDate.From != null)
                {
                    query = query.Where(t => t.CompleteDate >= filter.CompleteDate.From);
                }

                if (filter.CompleteDate.To != null)
                {
                    query = query.Where(t => t.CompleteDate <= filter.CompleteDate.To);
                }
            }

            if (filter.DueDate != null)
            {
                if (filter.DueDate.From != null)
                {
                    query = query.Where(t => t.DueDate >= filter.DueDate.From);
                }

                if (filter.DueDate.To != null)
                {
                    query = query.Where(t => t.DueDate <= filter.DueDate.To);
                }
            }

            if (filter.Status != null)
            {
                query = query.Where(pr => pr.Status == filter.Status);
            }

            if (filter.ProjectId != null)
            {
                query = query.Where(pr => pr.Project.Id == filter.ProjectId);
            }

            if (filter.Tag != null)
            {
                query = query.Where(pr => pr.Tags.Contains(filter.Tag));
            }

            if (filter.HasDueDate != null)
            {
                query = filter.HasDueDate == true ? query.Where(pr => pr.DueDate != null) : query.Where(pr => pr.DueDate == null);
            }

            return query;
        }
    }
}
