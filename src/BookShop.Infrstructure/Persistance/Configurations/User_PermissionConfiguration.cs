using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class User_PermissionConfiguration : IEntityTypeConfiguration<User_Permission>
    {
        public void Configure(EntityTypeBuilder<User_Permission> builder)
        {
            builder.ToTable("User_Permissions");
            builder.HasOne(a => a.Permission).WithMany(a => a.User_Permissions).HasForeignKey(a => a.PermissionId);
            builder.HasOne(a => a.User).WithMany(a => a.User_Permissions).HasForeignKey(a => a.UserId);

        }
    }
}
