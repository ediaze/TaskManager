namespace TaskManager.Application.Dtos
{
    public class TaskItemCreationDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
