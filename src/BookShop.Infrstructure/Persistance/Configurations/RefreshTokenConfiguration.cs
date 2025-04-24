using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefeshToken");
            
            //Relations
            builder.HasOne(a => a.User).WithMany(a => a.UserTokens).HasForeignKey(a => a.UserId);

            //Properties
            builder.Property(a => a.TokenValue).HasColumnType("VarChar(32)");
        
        
        }
    }
}
