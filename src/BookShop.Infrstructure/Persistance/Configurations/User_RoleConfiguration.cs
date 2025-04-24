using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    //public class User_RoleConfiguration : IEntityTypeConfiguration<User_Role>
    //{
    //    public void Configure(EntityTypeBuilder<User_Role> builder)
    //    {
    //        builder.ToTable("User_Roles");
    //        builder.HasOne(a => a.Role).WithMany(a => a.User_Roles).HasForeignKey(a => a.RoleId);
    //        builder.HasOne(a => a.User).WithMany(a => a.User_Roles).HasForeignKey(a => a.UserId);

    //    }
    //}
}
