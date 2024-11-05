using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IEBookRepository : IRepository,
        IReadRepository<EBook, Guid>,
        IWriteRepository<EBook, Guid>,
        IDeleteRepository<EBook, Guid>
    {
    }
}
