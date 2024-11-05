using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IFavoriteRepository :
        IRepository,
        IReadRepository<Favorite, Guid>,
        IWriteRepository<Favorite, Guid>,
        IDeleteRepository<Favorite, Guid>
    {
    }
}
