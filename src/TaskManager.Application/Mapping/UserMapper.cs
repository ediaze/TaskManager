using TaskManager.Application.Dtos;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mapping
{
    public static class UserMapper
    {
        public static User? ConvertToEntity(UserRegistrationDto userDto, byte[] passwordHash, byte[] passwordSalt)
        {
            if (userDto == null)
            {
                return null;
            }

            return new User
            {
                Username = userDto.Username,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
        }

        public static UserRegisterResultDto? ConvertToDto(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserRegisterResultDto
            {
                UserId = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }
        public static UserLoginResultDto? ConvertToDto(User user, string token)
        {
            if (user == null)
            {
                return null;
            }

            return new UserLoginResultDto
            {
                UserId = user.Id,
                Username = user.Username,
                Token = token
            };
        }
    }
}
