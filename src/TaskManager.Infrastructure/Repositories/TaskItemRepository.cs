using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Interfaces;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskItemRepository(TaskManagerDbContext context) : ITaskItemRepository
    {
        private readonly TaskManagerDbContext _context = context;

        public async Task<TaskItem?> AddAsync(TaskItem entry)
        {
            entry.Id = Guid.NewGuid();
            _context.TaskItems.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<int> DeleteAsync(TaskItem entry)
        {
            _context.TaskItems.Remove(entry);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected;
        }

        public async Task<IList<TaskItem>?> GetAllAsync()
        {
            IList<TaskItem>? entries = await _context.TaskItems.ToListAsync();
            return entries;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            TaskItem? entry = await _context.TaskItems.FindAsync(id);
            return entry;
        }

        public async Task<int?> UpdateAsync(TaskItem entry)
        {
            _context.Entry(entry).State = EntityState.Modified;

            try
            {
                var rowsAffected = await _context.SaveChangesAsync();
                return rowsAffected;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EntityExists(entry.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<bool> EntityExists(Guid id)
        {
            return await _context.TaskItems.AnyAsync(e => e.Id == id);
        }
    }
}