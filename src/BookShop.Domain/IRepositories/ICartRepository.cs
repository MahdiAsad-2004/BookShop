using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface ICartRepository :
        IRepository,
        IReadRepository<Cart, Guid>
    {

        public Task<Guid> Create(Cart cart);


        public Task<Guid?> GetIdForUser(Guid userId);

    }
}
