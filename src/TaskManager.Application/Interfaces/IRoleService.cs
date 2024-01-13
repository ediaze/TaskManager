using TaskManager.Application.Dtos;

namespace TaskManager.Application.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto?> AddRoleAsync(RoleDto role);
        Task<IList<RoleDto>?> GetAllAsync();
        Task<RoleDto?> GetByIdAsync(Guid id);
    }
}
