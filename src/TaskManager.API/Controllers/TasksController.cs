using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Dtos;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(ITaskService service) : ControllerBase
    {
        private readonly ITaskService _service = service;

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
        public async Task<ActionResult<TaskItemDto>> AddTaskItem([FromBody] TaskItemCreationDto taskItem)
        {
            var todoItem = await _service.AddAsync(taskItem);
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
