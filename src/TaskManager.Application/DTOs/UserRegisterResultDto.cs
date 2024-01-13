namespace TaskManager.Application.Dtos
{
    public class UserRegisterResultDto
    {
        public Guid UserId { get; set; }
        public required string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; } = null;
    }
}
