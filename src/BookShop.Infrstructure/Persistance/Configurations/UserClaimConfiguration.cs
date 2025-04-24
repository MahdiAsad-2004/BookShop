using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    //public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    //{
    //    public void Configure(EntityTypeBuilder<UserClaim> builder)
    //    {
    //        builder.ToTable("UserClaims");
            
    //        //Relations
    //        builder.HasOne(a => a.User).WithMany(a => a.UserClaims).HasForeignKey(a => a.UserId);

    //        //Properties
    //        builder.Property(a => a.Type).HasColumnType("VarChar(50)");
    //        builder.Property(a => a.Value).HasColumnType("VarChar(50)");
    //    }
    //}
}
