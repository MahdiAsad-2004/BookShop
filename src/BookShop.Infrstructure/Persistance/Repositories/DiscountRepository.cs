using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class DiscountRepository : CrudRepository<Discount, Guid> , IDiscountRepository
    {
        public DiscountRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }

        public async Task<bool> IsExist(Guid id)
        {
            if (id == Guid.Empty)
                return false;
            return await _dbSet.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> IsExist(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            return await _dbSet.AnyAsync(a => a.Name == name);
        }

        public async Task<bool> IsExist(string name, Guid exceptId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            return await _dbSet.AnyAsync(a => a.Name == name && a.Id != exceptId);
        }



    }
}
