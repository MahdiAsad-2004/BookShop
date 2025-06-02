using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IOrderStatusRepository :
        IRepository,
        IReadRepository<OrderStatus, Guid>
    {


    }
}
