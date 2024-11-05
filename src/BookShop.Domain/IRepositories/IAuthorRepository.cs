using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IAuthorRepository :
        IRepository,
        IReadRepository<Author, Guid>,
        IWriteRepository<Author, Guid>,
        IDeleteRepository<Author, Guid>
    {
    }
}
