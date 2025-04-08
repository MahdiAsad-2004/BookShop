using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IPermissionRepository :
        IRepository,
        IReadRepository<Permission, Guid>
    {

        public Task<Permission[]> GetUserPermissions(Guid userId);


    }
}
