using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Dtos;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService service) : ControllerBase
    {
        private readonly IUserService _userService = service;

        [Authorize(Roles = "Super-Admin")]
        [HttpPost("register")]
        public async Task<ActionResult<UserRegisterResultDto?>> Register(UserRegistrationDto userDto)
        {
            return await _userService.Register(userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResultDto?>> Login(UserLoginDto userDto)
        {
            return await _userService.Login(userDto);
        }

        [Authorize(Roles = "Super-Admin")]
        [HttpPost("{userId}/roles/{roleId}")]
        public async Task<IActionResult> AddRoleToUser(Guid userId, Guid roleId)
        {
            _ = await _userService.AddRoleToUserAsync(userId, roleId);
            return Ok();
        }
    }
}
