using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("TaskManager/[controller]")]
    public class TasksController(ITaskItemService service) : ControllerBase
    {
        private readonly ITaskItemService _service = service;

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAllTaskItems()
        {
            var taskItems = await _service.GetAllAsync();
            return Ok(taskItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDto>> GetTaskItem(Guid id)
        {
            var taskItem = await _service.GetByIdAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            return Ok(taskItem);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> AddTaskItem([FromBody] CreateTaskItemDto taskItem)
        {
            var todoItem = await _service.CreateAsync(taskItem);
            return CreatedAtAction(
                nameof(GetTaskItem),
                new { id = todoItem?.Id },
                todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(Guid id, [FromBody] TaskItemDto taskItem)
        {
            await _service.UpdateAsync(id, taskItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
