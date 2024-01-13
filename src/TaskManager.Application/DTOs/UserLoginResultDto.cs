namespace TaskManager.Application.Dtos
{
    public class UserLoginResultDto
    {
        public Guid UserId { get; set; }
        public required string Username { get; set; }
        public required string Token { get; set; } // JWT token
    }
}
