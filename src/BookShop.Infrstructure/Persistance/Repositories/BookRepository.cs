using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Infrastructure.Persistance.Repositories.Common;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class BookRepository : CrudRepository<Book, Guid> , IBookRepository
    {
        public BookRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }

    }
}
