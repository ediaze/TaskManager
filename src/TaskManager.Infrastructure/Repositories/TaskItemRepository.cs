using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskItemRepository(TaskManagerDbContext context) : ITaskItemRepository
    {
        private readonly TaskManagerDbContext _context = context;

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            TaskItem? taskItem = await _context.TaskItems.FindAsync(id);
            return taskItem;
        }

        public async Task<IList<TaskItem>?> GetAllAsync()
        {
            IList<TaskItem>? taskItems = await _context.TaskItems.ToListAsync();
            return taskItems;
        }

        public async Task<TaskItem?> AddAsync(TaskItem taskItem)
        {
            taskItem.Id = Guid.NewGuid();
            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<int?> UpdateAsync(TaskItem taskItem)
        {
            _context.Entry(taskItem).State = EntityState.Modified;

            try
            {
                var rowsAffected = await _context.SaveChangesAsync();
                return rowsAffected;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(taskItem.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<int> DeleteAsync(TaskItem taskItem)
        {
            _context.TaskItems.Remove(taskItem);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected;
        }

        private bool TaskItemExists(Guid id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}
