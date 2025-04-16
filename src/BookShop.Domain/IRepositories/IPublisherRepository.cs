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

        Task<bool> IsExist(Guid id);

        Task<bool> IsExist(string title);

        Task<bool> IsExist(string title , Guid exceptId);




    }
}
