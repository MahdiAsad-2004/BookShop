using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class CartRepository : CrudRepository<Cart, Guid>, ICartRepository
    {
        public CartRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }


        public async Task<Guid?> GetIdForUser(Guid userId)
        {
            var Ids = await _dbSet.Where(a => a.UserId == userId).Select(a => a.Id).ToListAsync();
            
            if (Ids.Any())
                return Ids[0];
            
            return null;
        }

        public async Task<Guid> Create(Cart cart)
        {
            SetPropertiesForCreate(cart, Guid.NewGuid(), _currentUser.Id.ToString());
            await _dbSet.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
            await cart.PublishAllDomainEventsAndClear(_domainEventPublisher);
            return cart.Id;
        }

    }
}
