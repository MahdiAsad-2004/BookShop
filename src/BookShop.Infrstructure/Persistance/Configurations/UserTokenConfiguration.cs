using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class UserTokenonfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserTokens");
            
            //Relations
            builder.HasOne(a => a.User).WithMany(a => a.UserTokens).HasForeignKey(a => a.UserId);

            //Properties
            builder.Property(a => a.LoginProvider).HasColumnType("VarChar(50)");
            builder.Property(a => a.TokenName).HasColumnType("VarChar(50)");
            builder.Property(a => a.TokenValue).HasColumnType("VarChar(50)");
        
        
        }
    }
}
