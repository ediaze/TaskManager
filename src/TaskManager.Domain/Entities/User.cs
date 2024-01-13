using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.Entities
{
    public class User: IEntityWithIdGuid
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
            Roles = new HashSet<Role>();
        }
    }
}
