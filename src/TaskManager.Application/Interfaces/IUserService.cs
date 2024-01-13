using System.Linq.Expressions;
using TaskManager.Application.Dtos;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserRegisterResultDto?> Register(UserRegistrationDto userDto);
        Task<UserLoginResultDto?> Login(UserLoginDto userDto);
        Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);
        Task<int?> AddRoleToUserAsync(Guid userId, Guid roleId);
    }
}
