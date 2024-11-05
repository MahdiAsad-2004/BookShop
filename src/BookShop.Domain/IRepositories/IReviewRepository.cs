using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IReviewRepository :
        IRepository,
        IReadRepository<Review, Guid>,
        IWriteRepository<Review, Guid>,
        IDeleteRepository<Review, Guid>
    {
    }
}
