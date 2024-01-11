using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces
{
    public interface ITaskItemService
    {
        Task<TaskItemDto?> GetByIdAsync(Guid id);
        Task<IList<TaskItemDto>?> GetAllAsync();
        Task<TaskItemDto?> CreateAsync(CreateTaskItemDto taskDto);
        Task<int?> UpdateAsync(Guid id, TaskItemDto taskDto);
        Task<int> DeleteAsync(Guid id);
    }
}
