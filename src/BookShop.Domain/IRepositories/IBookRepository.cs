using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IBookRepository :
        IRepository,
        IReadRepository<Book, Guid>,
        IWriteRepository<Book, Guid>,
        IDeleteRepository<Book, Guid>
    {
    }
}
