using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Repositories
{
    public interface ITaskItemRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<IList<TaskItem>?> GetAllAsync();
        Task<TaskItem?> AddAsync(TaskItem task);
        Task<int?> UpdateAsync(TaskItem taskItem);
        Task<int> DeleteAsync(TaskItem taskItem);
    }
}