using BookShop.Domain.Constants;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistance.SeedDatas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace BookShop.Infrastructure.Persistance.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            
            //relations
            builder.HasMany(a => a.User_Permissions).WithOne(a => a.Permission).HasForeignKey(a => a.PermissionId).OnDelete(DeleteBehavior.Restrict);

            //properties
            builder.Property(a => a.Name).HasColumnType("VarChar(30)");



            


        }
    }
}
