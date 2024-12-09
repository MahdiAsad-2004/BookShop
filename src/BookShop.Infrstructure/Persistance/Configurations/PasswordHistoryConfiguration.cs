using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class PasswordHistoryConfiguration : IEntityTypeConfiguration<PasswordHistory>
    {
        public void Configure(EntityTypeBuilder<PasswordHistory> builder)
        {
            builder.ToTable("PasswordHistories");
            
            //relations
            builder.HasOne(a => a.User).WithMany(a => a.PasswordHistories).HasForeignKey(a => a.UserId);

            //properties
            builder.Property(a => a.PasswordHash).HasColumnType("VarChar(300)");
        
        }
    }
}
