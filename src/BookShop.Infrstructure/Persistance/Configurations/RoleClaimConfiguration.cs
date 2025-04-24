using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    //public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    //{
    //    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    //    {
    //        builder.ToTable("RoleClaims");
            
    //        //Relations
    //        builder.HasOne(a => a.Role).WithMany(a => a.RoleClaims).HasForeignKey(a => a.RoleId).HasPrincipalKey(a => a.Id)
    //            .OnDelete(DeleteBehavior.Cascade);

    //        //Properties
    //        builder.Property(a => a.Type).HasColumnType("VarChar(50)");
    //        builder.Property(a => a.Value).HasColumnType("VarChar(50)");


    //    }
    //}
}
