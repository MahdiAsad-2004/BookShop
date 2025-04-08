
using BookShop.Domain.Constants;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistance.SeedDatas;

namespace BookShop.Infrstructure.Persistance.SeedDatas
{
    public static class PermissionsSeed
    {

        public static List<Permission> GetPermissions(Guid adminId)
        {
            List<Permission> permissions = new List<Permission>();  
            foreach (var permissionName in PermissionConstants.GetAll())
            {
                permissions.Add(new Permission
                {
                    Name = permissionName,
                    CreateBy = adminId.ToString(),
                    CreateDate = DateTime.UtcNow,
                    DeleteDate = null,
                    DeletedBy = null,
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    LastModifiedBy = adminId.ToString(),
                    LastModifiedDate = DateTime.UtcNow,
                });
            }
            return permissions;
        }


    }
}
