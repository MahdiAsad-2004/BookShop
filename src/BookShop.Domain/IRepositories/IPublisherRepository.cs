using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IPublisherRepository :
        IRepository,
        IReadRepository<Publisher, Guid>,
        IWriteRepository<Publisher, Guid>,
        IDeleteRepository<Publisher, Guid>
    {
    }
}
