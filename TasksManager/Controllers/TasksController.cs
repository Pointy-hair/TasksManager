using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TasksManager.DataAccess.Tasks;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tasks;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ListResponse<TaskResponse>))]
        public async Task<IActionResult> GetTasksListAsync(TaskFilter filter, ListOptions options,
            [FromServices] ITasksListQuery query)
        {
            var response = await query.RunAsync(filter, options);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TaskResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskRequest request,
            [FromServices] ICreateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TaskResponse response;
            try
            {
                response = await command.ExecuteAsync(request);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }

            return StatusCode(201, response);
        }

        [HttpGet("{taskId}")]
        [ProducesResponseType(200, Type = typeof(TaskResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTaskAsync(int taskId, [FromServices] ITaskQuery query)
        {
            TaskResponse response = await query.RunAsync(taskId);
            return response == null
                ? (IActionResult) NotFound()
                : Ok(response);
        }

        [HttpPut("{taskId}")]
        [ProducesResponseType(200, Type = typeof(TaskResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTaskAsync(int taskId, [FromBody] UpdateTaskRequest request,
            [FromServices] IUpdateTaskCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TaskResponse response;
            try
            {
                response = await command.ExecuteAsunc(taskId, request);
            }

            catch (Exception e)
            {
                return NotFound(e.Message);
            }

            return Ok(response);
        }

        [HttpDelete("{taskId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTaskAsync(int taskId, [FromServices] IDeleteTaskCommand command)
        {
            try
            {
                await command.ExecuteAsync(taskId);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }

            return StatusCode(204);
        }

        [HttpDelete("{taskId}/tags/{tagId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTagInTaskAsync(int taskId, int tagId,
            [FromServices] IDeleteTagInTaskCommand command)
        {
            try
            {
                await command.ExecuteAsync(taskId, tagId);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }

            return StatusCode(204);
        }
    }
}
