using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

            entity.HasData(
                new Role { Id = Guid.Parse("862d986b-a82d-490f-aaa1-ed66b6971a64"), Name = "Super-Admin" },
                new Role { Id = Guid.Parse("bad7f2b1-6a30-4c2c-b027-12f089c09925"), Name = "Manager" },
                new Role { Id = Guid.Parse("4b0fd5b7-ea81-4a3b-b2fa-351bcc66ca12"), Name = "Employee" },
                new Role { Id = Guid.Parse("4fef154f-064c-4b0e-9096-c775563117f0"), Name = "Member" },
                new Role { Id = Guid.Parse("23b5bb6a-7507-4d5d-ae73-2e4151112a67"), Name = "Customer" }
            );
        }
    }
}
