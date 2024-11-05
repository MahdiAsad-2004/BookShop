using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IRoleRepository :
        IRepository,
        IReadRepository<Role, Guid>,
        IWriteRepository<Role, Guid>,
        IDeleteRepository<Role, Guid>
    {


        Task<Role> GetByNameAsync(string name);

        Task<Role> GetByNormalizedNameOrDefaultAsync(string normalizedName);



    }
}
