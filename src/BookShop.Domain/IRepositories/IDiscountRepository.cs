using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IDiscountRepository :
        IRepository,
        IReadRepository<Discount, Guid>,
        IWriteRepository<Discount, Guid>,
        IDeleteRepository<Discount, Guid>
    {

        Task<bool> IsExist(Guid id);

        Task<bool> IsExist(string name);

        Task<bool> IsExist(string name,Guid exceptId);



    }
}
