using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;
using BookShop.Domain.QueryOptions;

namespace BookShop.Domain.IRepositories
{
    public interface ICartItemRepository :
        IRepository,
        IReadRepository<CartItem, Guid>
    {

        public Task Create(CartItem cartItem);

        public Task<bool> IsExist(Guid produtcId,Guid cartId);

        public Task<Guid?> GetId(Guid produtcId,Guid cartId);

        public Task<bool> Update(Guid id, int quntity);

        public Task<List<CartItem>> GetAll(CartItemQueryOption queryOption, CartItemSortingOrder? sortingOrder = null);


        public Task<bool> SoftDelete(Guid id);

    }
}
