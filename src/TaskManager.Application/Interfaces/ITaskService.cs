using TaskManager.Application.Dtos;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskItemDto?> GetByIdAsync(Guid id);
        Task<IList<TaskItemDto>?> GetAllAsync();
        Task<TaskItemDto?> AddAsync(TaskItemCreationDto taskDto);
        Task<int?> UpdateAsync(Guid id, TaskItemDto taskDto);
        Task<int> DeleteAsync(Guid id);
    }
}
