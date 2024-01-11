using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces
{
    public interface IUserService
    {
        Task<TaskItemDto?> CreateAsync(CreateTaskItemDto taskDto);
        Task<TaskItemDto?> AuthenticateAsync(Guid id);
    }
}
