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
            
            //Relations
            builder.HasMany(a => a.RoleClaims).WithOne(a => a.Role).HasForeignKey(a => a.RoleId);
            builder.HasMany(a => a.User_Roles).WithOne(a => a.Role).HasForeignKey(a => a.RoleId);

            //Properties
            builder.Property(a => a.Name).HasColumnType("VarChar(30)");
            builder.Property(a => a.NormalizedName).HasColumnType("VarChar(30)");
            builder.Property(a => a.ConcurrencyStamp).HasColumnType("VarChar(50)");

        }
    }
}
