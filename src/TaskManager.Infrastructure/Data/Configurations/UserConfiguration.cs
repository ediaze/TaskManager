using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Helpers;

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

            var adminUserId = Guid.Parse("9321ac6a-1a01-456c-b89c-4fd3af78fdf2");
            var adminRoleId = Guid.Parse("862d986b-a82d-490f-aaa1-ed66b6971a64"); // Super-Admin

            var password = Environment.GetEnvironmentVariable("Authentication_SuperAdminPassword")
                            ?? throw new ArgumentException("Authentication_SuperAdminPassword");
            PasswordHasher.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            entity.HasData(
                new User
                {
                    Id = adminUserId,
                    Username = "admin",
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@example.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                }
            );

            // Configure many-to-many relationship
            entity.HasMany(u => u.Roles)
                  .WithMany(r => r.Users)
                  .UsingEntity(j => j.HasData(new { UsersId = adminUserId, RolesId = adminRoleId }));
        }
    }
}
