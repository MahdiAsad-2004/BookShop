using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class PublisherRepository : CrudRepository<Publisher, Guid> , IPublisherRepository
    {
        public PublisherRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }

        public async Task<bool> IsExist(Guid id)
        {
            if (id == Guid.Empty)
                return false;
            return await _dbSet.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> IsExist(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return false;
            return await _dbSet.AnyAsync(a => a.Title == title);
        }

        public async Task<bool> IsExist(string title, Guid exceptId)
        {
            if (string.IsNullOrWhiteSpace(title))
                return false;
            return await _dbSet.AnyAsync(a => a.Title == title && a.Id != exceptId);
        }
    }
}
