using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using TaskManager.Application.Dtos;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Helpers;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Mapping;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Interfaces;

namespace TaskManager.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly string _secretKey;
        private readonly string _tokenExpirationDays;

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;

            const string _tokenKey = "Authentication_TokenKey";
            const string _expirationDays = "Authentication_TokenExpirationDays";
            _secretKey = Environment.GetEnvironmentVariable(_tokenKey) ?? throw new ArgumentException(_tokenKey);
            _tokenExpirationDays = Environment.GetEnvironmentVariable(_expirationDays) ?? throw new ArgumentException(_expirationDays);
        }

        public async Task<UserRegisterResultDto?> Register(UserRegistrationDto userDto)
        {
            if (await ExistsAsync(u => u.Username == userDto.Username))
            {
                throw new ApiException("Username is already taken.");
            }

            if (await ExistsAsync(u => u.Email == userDto.Email))
            {
                throw new ApiException("Email is already in use.");
            }

            if (!PasswordHasher.ValidatePassword(userDto.Password, out string errorMessage))
            {
                throw new ApiException(errorMessage);
            }

            PasswordHasher.CreatePasswordHash(userDto.Password, 
                out byte[] passwordHash, out byte[] passwordSalt);

            var user = UserMapper.ConvertToEntity(userDto, passwordHash, passwordSalt) 
                ?? throw new ApiException("Convertion to User is null.");
            var entry = await _userRepository.AddAsync(user) ?? throw new ApiException(HttpStatusCode.NotFound);

            return UserMapper.ConvertToDto(entry);
        }

        public async Task<UserLoginResultDto?> Login(UserLoginDto userDto)
        {
            var user = (await _userRepository.GetAllAsync(u => u.Username == userDto.Username))?
                .FirstOrDefault();

            if (user == null || !PasswordHasher.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new ApiException("Username or password is incorrect.");
            }

            var token = GenerateJwtToken(user);
            return UserMapper.ConvertToDto(user, token);
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
        {
            var exists = await _userRepository.ExistsAsync(predicate);
            return exists;
        }

        public async Task<int?> AddRoleToUserAsync(Guid userId, Guid roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var role = await _roleRepository.GetByIdAsync(roleId);

            if (user == null || role == null)
            {
                throw new ApiException("User or Role does not exist");
            }

            if (user.Roles.Any(r => r.Id == roleId))
            {
                throw new ApiException("User already has the role");
            }

            user.Roles.Add(role);
            return await _userRepository.UpdateAsync(user);
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
            _ = int.TryParse(_tokenExpirationDays, out int tokenExpirationDays);

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (JwtRegisteredClaimNames.NameId, user.Username),
                new (ClaimTypes.Name, user.Username),
                new (JwtRegisteredClaimNames.UniqueName, user.Id.ToString())
            };

            if (user.Roles != null)
            {
                // Add role claims
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }
            }

            if (user.Email != null)
            {
                claims.Add(new (JwtRegisteredClaimNames.Email, user.Email));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(tokenExpirationDays),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
