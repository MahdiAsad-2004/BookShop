using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface ICategoryRepository :
        IRepository,
        IReadRepository<Category, Guid>,
        IWriteRepository<Category, Guid>,
        IDeleteRepository<Category, Guid>
    {
    }
}
