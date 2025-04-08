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
            
            //Relations
            builder.HasMany(a => a.AuditLogs).WithOne(a => a.User).HasForeignKey(a => a.UserId); 
            builder.HasMany(a => a.UserClaims).WithOne(a => a.User).HasForeignKey(a => a.UserId); 
            builder.HasMany(a => a.User_Roles).WithOne(a => a.User).HasForeignKey(a => a.UserId); 
            builder.HasMany(a => a.Favorites).WithOne(a => a.User).HasForeignKey(a => a.UserId); 
            builder.HasMany(a => a.Reviews).WithOne(a => a.User).HasForeignKey(a => a.UserId); 
            builder.HasMany(a => a.PasswordHistories).WithOne(a => a.User).HasForeignKey(a => a.UserId);
            builder.HasMany(a => a.UserTokens).WithOne(a => a.User).HasForeignKey(a => a.UserId);
            builder.HasMany(a => a.User_Permissions).WithOne(a => a.User).HasForeignKey(a => a.UserId);

            //Properties
            builder.Property(a => a.Name).HasColumnType("NVarChar(30)");
            builder.Property(a => a.Username).HasColumnType("VarChar(30)");
            builder.Property(a => a.Email).HasColumnType("VarChar(100)");
            builder.Property(a => a.NormalizedEmail).HasColumnType("VarChar(100)");
            builder.Property(a => a.NormalizedUsername).HasColumnType("VarChar(30)");
            builder.Property(a => a.ConcurrencyStamp).HasColumnType("VarChar(50)");
            builder.Property(a => a.SecurityStamp).HasColumnType("VarChar(50)");
            builder.Property(a => a.PasswordHash).HasColumnType("VarChar(300)");
            builder.Property(a => a.ImageName).HasColumnType("VarChar(50)");
            builder.Property(a => a.PhoneNumber).HasColumnType("VarChar(11)");


        }
    }
}
