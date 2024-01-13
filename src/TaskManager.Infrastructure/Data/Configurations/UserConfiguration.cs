using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);

            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.PasswordSalt).IsRequired();

            // Optional fields
            entity.Property(e => e.FirstName).IsRequired(false).HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired(false).HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired(false).HasMaxLength(300);

            // Indixes and constraints
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasIndex(u => u.Username).IsUnique();

            // Relationships
            entity.HasMany(u => u.Roles)
                  .WithMany(r => r.Users);
        }
    }
}
