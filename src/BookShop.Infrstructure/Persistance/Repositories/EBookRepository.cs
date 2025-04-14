using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Infrastructure.Persistance.Repositories.Common;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class EBookRepository : CrudRepository<EBook, Guid> , IEBookRepository
    {
        public EBookRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }



        public async Task Add(EBook ebook, Product product, Guid[] authorIds)
        {
            DateTime dateTime = DateTime.UtcNow;
            product.Id = Guid.NewGuid();
            product.CreateDate = product.LastModifiedDate = dateTime;
            product.CreateBy = product.LastModifiedBy = _currentUser.GetId();
            //---------------------------------------------------------------------
            ebook.Id = Guid.NewGuid();
            ebook.CreateDate = ebook.LastModifiedDate = dateTime;
            ebook.CreateBy = ebook.LastModifiedBy = _currentUser.GetId();
            //---------------------------------------------------------------------
            List<Author_EBook> author_Ebooks = new List<Author_EBook>();
            if (authorIds != null)
            {
                foreach (Guid authorId in authorIds)
                {
                    author_Ebooks.Add(new Author_EBook
                    {
                        CreateBy = _currentUser.GetId(),
                        CreateDate = dateTime,
                        LastModifiedBy = _currentUser.GetId(),
                        LastModifiedDate = dateTime,
                        AuthorId = authorId,
                        EBook = ebook,
                        EBookId = ebook.Id,
                        Id = Guid.NewGuid(),
                    });
                }
            }
            //---------------------------------------------------------------------
            ebook.Product = product;
            ebook.Author_EBooks = author_Ebooks;
            await _dbContext.EBooks.AddAsync(ebook);
            await _dbContext.SaveChangesAsync();
            await ebook.PublishAllDomainEventsAndClear(_domainEventPublisher);
        }



    }
}
