using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.Entities
{
    public class Role: IEntityWithIdGuid
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Role()
        {
            Id = Guid.NewGuid();
            Users = new HashSet<User>();
        }
    }
}
