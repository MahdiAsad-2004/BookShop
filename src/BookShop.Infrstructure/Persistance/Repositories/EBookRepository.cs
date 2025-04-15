using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using BookShop.Infrstructure.Persistance.QueryOptions;
using Microsoft.EntityFrameworkCore;

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




        public async Task<EBook> Get(Guid id, EBookQueryOption queryOption)
        {
            //return await GetWithQuery(id);
            return await GetWithLinq(id, queryOption);
        }






        private async Task<EBook> GetWithLinq(Guid id, EBookQueryOption queryOption)
        {
            var bookQueryObject = await _dbContext.EBooks.AsNoTracking()
            .Select(ebook => new EBookQueryResultObject
            {
                EBook = ebook,
                Product = queryOption.IncludeProduct == false ? null : _dbContext.Products.First(a => a.Id == ebook.ProductId),
                Reviews = queryOption.IncludeReviews == false ? null : _dbContext.Reviews.Where(a => a.ProductId == ebook.ProductId).ToList(),
                Publisher = queryOption.IncludePublisher == false ? null : _dbContext.Publishers.First(a => a.Id == ebook.PublisherId),
                Translator = queryOption.IncludeTranslator == false ? null : _dbContext.Translators.FirstOrDefault(a => a.Id == ebook.TranslatorId),
                Author_EBooks = queryOption.IncludeAuthors == false ? null : ebook.Author_EBooks.ToArray(),
                MostPriorityValidDiscount = queryOption.IncludeDiscounts == false ? null : _dbContext.Product_Discounts
                    .Where(pd => pd.ProductId == ebook.ProductId)
                    .Join(_dbContext.Discounts,
                        pd => pd.DiscountId,
                        d => d.Id,
                        (pd, d) => new Discount
                        {
                            DiscountPercent = d.DiscountPercent,
                            DiscountPrice = d.DiscountPrice,
                            Priority = d.Priority,
                            StartDate = d.StartDate,
                            EndDate = d.EndDate,
                            UsedCount = d.UsedCount,
                            MaximumUseCount = d.MaximumUseCount,
                        })
                        .Where(
                            d =>
                                (d.StartDate == null || d.StartDate <= DateTime.Now) &&
                                (d.EndDate == null || d.EndDate >= DateTime.Now) &&
                                (d.MaximumUseCount == null || d.MaximumUseCount > d.UsedCount) &&
                                (d.DiscountPercent != null || d.DiscountPrice != null)
                    )
                    .OrderBy(d => d.Priority)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(a => a.EBook.Id == id);

            if (bookQueryObject == null)
                throw new NotFoundException($"EBook with id '{id}' not found");

            return bookQueryObject.MapToBook();
        }





    }
}
