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
            builder.HasMany(a => a.User_Permissions).WithOne(a => a.Permission).HasForeignKey(a => a.PermissionId);





            foreach (var permissionName in Permissions.GetAll())
            {
                builder.HasData(new Permission
                {
                    Name = permissionName,
                    CreateBy = UsersSeed.SuperAdmin.Id.ToString(),
                    CreateDate = DateTime.UtcNow,
                    DeleteDate = null,
                    DeletedBy = null,
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    LastModifiedBy = UsersSeed.SuperAdmin.Id.ToString(),
                    LastModifiedDate = DateTime.UtcNow,
                });
            }


        }
    }
}
