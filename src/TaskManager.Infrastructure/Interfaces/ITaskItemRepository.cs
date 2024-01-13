using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Interfaces
{
    public interface ITaskItemRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<IList<TaskItem>?> GetAllAsync();
        Task<TaskItem?> AddAsync(TaskItem entry);
        Task<int?> UpdateAsync(TaskItem entry);
        Task<int> DeleteAsync(TaskItem entry);
    }
}