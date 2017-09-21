using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TasksManager.DataAccess.Tags;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tags;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ListResponse<TagResponse>))]
        public async Task<IActionResult> GetTagsListAsync(TagFilter filter, ListOptions options, 
            [FromServices] ITagsListQuery query)
        {
            var response = await query.RunAsync(filter, options);
            return Ok(response);
        }

        [HttpDelete("{tagId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteTagAsync(int tagId, [FromServices] IDeleteTagCommand command)
        {
            try
            {
                await command.ExecuteAsync(tagId);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }

            return StatusCode(204);
        }
    }
}
