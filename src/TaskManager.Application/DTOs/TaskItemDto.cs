using TaskManager.Application.Dtos;

namespace TaskManager.Application.DTOs
{
    public class TaskItemDto: TaskItemCreationDto
    {
        public Guid Id { get; set; }
    }
}
