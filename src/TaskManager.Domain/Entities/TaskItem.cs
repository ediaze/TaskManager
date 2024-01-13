using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.Entities
{
    public class TaskItem: IEntityWithIdGuid
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}