using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mapping
{
    public static class TaskItemMapper
    {
        public static TaskItemDto? ConvertToDto(TaskItem taskItem)
        {
            if (taskItem == null)
            {
                return null;
            }

            return new TaskItemDto
            {
                Id = taskItem.Id,
                Name = taskItem.Name,
                Description = taskItem.Description,
                DueDate = taskItem.DueDate,
                IsCompleted = taskItem.IsCompleted
            };
        }

        public static TaskItem? ConvertToEntity(TaskItemDto taskItem)
        {
            if (taskItem == null)
            {
                return null;
            }

            return new TaskItem
            {
                Id = taskItem.Id,
                Name = taskItem.Name,
                Description = taskItem.Description,
                DueDate = taskItem.DueDate,
                IsCompleted = taskItem.IsCompleted
            };
        }
    }
}
