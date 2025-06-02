using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;
using BookShop.Domain.QueryOptions;

namespace BookShop.Domain.IRepositories
{
    public interface IFavoriteRepository :
        IRepository,
        //IReadRepository<Favorite, Guid>,
        IWriteRepository<Favorite, Guid>
        //IDeleteRepository<Favorite, Guid>
    {

        Task<bool> IsExist(Guid userId, Guid productId);

        Task<Favorite[]> GetAll(FavoriteQueryOption queryOption);


    }
}
