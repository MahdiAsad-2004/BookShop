using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IOrderRepository :
        IRepository,
        IReadRepository<Order, Guid>
    {


    }
}
