using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TasksManager.DataAccess.Tags;
using TasksManager.Db;
using TasksManager.Entities;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tags;

namespace TasksManager.DataAccess.DbImplementation.Tags
{
    public class TagsListQuery : ITagsListQuery
    {
        private TasksContext Context { get; }
        private IMapper Mapper { get; }

        public TagsListQuery(TasksContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<ListResponse<TagResponse>> RunAsync(TagFilter filter, ListOptions options)
        {
            IQueryable<TagResponse> query = Context.Set<Tag>()
                .Include(t => t.Tasks)
                .ThenInclude(t => t.Task)
                .Select(t => Mapper.Map<Tag, TagResponse>(t))
                ;

            query = ApplyFilter(query, filter);

            int totalCount = await query.CountAsync();
            if (options.Sort == null)
            {
                options.Sort = "Id";
            }

            query = options.ApplySort(query);
            query = options.ApplyPaging(query);

            return new ListResponse<TagResponse>
            {
                Items = await query.ToListAsync(),
                Page = options.Page,
                PageSize = options.PageSize ?? totalCount,
                Sort = options.Sort,
                TotalItemsCount = totalCount
            };
        }

        private IQueryable<TagResponse> ApplyFilter(IQueryable<TagResponse> query, TagFilter filter)
        {
            if (filter.Name != null)
            {
                query = query.Where(n => n.Name.StartsWith(filter.Name));
            }
            return query;
        }
    }
}
