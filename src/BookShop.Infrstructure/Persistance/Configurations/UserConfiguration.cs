using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistance.SeedDatas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasMany(a => a.AuditLogs).WithOne(a => a.User).HasForeignKey(a => a.UserId); 
            builder.HasMany(a => a.UserClaims).WithOne(a => a.User).HasForeignKey(a => a.UserId); 
            builder.HasMany(a => a.User_Roles).WithOne(a => a.User).HasForeignKey(a => a.UserId); 
            builder.HasMany(a => a.Favorites).WithOne(a => a.User).HasForeignKey(a => a.UserId); 
            builder.HasMany(a => a.Reviews).WithOne(a => a.User).HasForeignKey(a => a.UserId); 
            builder.HasMany(a => a.PasswordHistories).WithOne(a => a.User).HasForeignKey(a => a.UserId);
            builder.HasMany(a => a.UserTokens).WithOne(a => a.User).HasForeignKey(a => a.UserId);
            builder.HasMany(a => a.User_Permissions).WithOne(a => a.User).HasForeignKey(a => a.UserId);



        }
    }
}
