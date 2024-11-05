using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasMany(a => a.RoleClaims).WithOne(a => a.Role).HasForeignKey(a => a.RoleId);
            builder.HasMany(a => a.User_Roles).WithOne(a => a.Role).HasForeignKey(a => a.RoleId);

        }
    }
}
