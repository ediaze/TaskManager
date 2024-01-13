using System.Net;
using TaskManager.Application.Dtos;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Interfaces;

namespace TaskManager.Application.Services
{
    public class RoleService(IGenericRepository<Role> roleRepository) : IRoleService
    {
        private readonly IGenericRepository<Role> _repository = roleRepository;

        public async Task<RoleDto?> AddRoleAsync(RoleDto roleDto)
        {
            var role = new Role { 
                Name = roleDto.Name 
            };

            var entity = await _repository.AddAsync(role) 
                ?? throw new ApiException(HttpStatusCode.NotFound);
            roleDto.Id = entity.Id;

            return roleDto;
        }

        public async Task<IList<RoleDto>?> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync() 
                ?? throw new ApiException(HttpStatusCode.NotFound);
            var items = entities.Select(e => new RoleDto
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();

            return items;
        }

        public async Task<RoleDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id) 
                ?? throw new ApiException(HttpStatusCode.NotFound);
            return new RoleDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
