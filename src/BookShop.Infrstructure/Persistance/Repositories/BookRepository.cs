using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class BookRepository : CrudRepository<Book, Guid>, IBookRepository
    {
        public BookRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }


        public async Task<Book> Get(Guid id, BookQueryOption queryOption)
        {
            //return await GetWithLinq(id);
            return await GetWithQuery(id , queryOption);
        }


        public class BookQueryResultObject
        {
            public Book Book { get; set; }
            public Product Product { get; set; }
            public Discount? MostPriorityValidDiscount { get; set; }
            public List<Review>? Reviews { get; set; }
            public Publisher? Publisher { get; set; }
            public Translator? Translator { get; set; }
            public List<Author>? Authors { get; set; }


            public Book MapToBook()
            {
                Book book = Book;
                book.Product = Product;
                book.Publisher = Publisher;
                book.Translator = Translator;
                book.Authors = Authors;
                book.Product.Reviews = Reviews;
                book.Product.DiscountedPrice = MostPriorityValidDiscount?.CalculateDiscountedPrice(Product.Price);
                
                return book;
            }

        }



        private async Task<Book> GetWithQuery(Guid id , BookQueryOption queryOption)
        {
            var bookQueryObject = await _dbContext.Books
            .Select(book => new BookQueryResultObject
            {
                Book = book,
                Product = _dbContext.Products.First(a => a.Id == book.ProductId),
                Reviews = queryOption.IncludeReviews == false ? null : _dbContext.Reviews.Where(a => a.ProductId == book.ProductId).ToList(),
                Publisher = queryOption.IncludePublisher == false ? null : _dbContext.Publishers.First(a => a.Id == book.PublisherId),
                Translator = queryOption.IncludeTranslator == false ? null : _dbContext.Translators.FirstOrDefault(a => a.Id == book.TranslatorId),
                Authors = queryOption.IncludeAuthors == false ? null : _dbContext.Authors.ToList(),
                MostPriorityValidDiscount = queryOption.IncludeDiscounts == false ? null : _dbContext.Product_Discounts
                    .Where(pd => pd.ProductId == book.ProductId)
                    .Join(_dbContext.Discounts,
                        pd => pd.DiscountId,
                        d => d.Id,
                        (pd, d) => new Discount{ 
                            DiscountPercent = d.DiscountPercent,
                            DiscountPrice = d.DiscountPrice,
                            Priority = d.Priority,
                            StartDate = d.StartDate,           
                            EndDate = d.EndDate , 
                            UsedCount = d.UsedCount ,
                            MaximumUseCount = d.MaximumUseCount,
                        })
                    .Where
                    (d => 
                        (d.StartDate == null || d.StartDate <= DateTime.Now) &&
                        (d.EndDate == null || d.EndDate >= DateTime.Now) &&
                        (d.MaximumUseCount == null || d.MaximumUseCount > d.UsedCount) &&
                        (d.DiscountPercent != null || d.DiscountPrice != null)
                    )
                    .OrderBy(d => d.Priority)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(a => a.Book.Id == id);

            if(bookQueryObject == null)
                throw new NotFoundException($"Book with id '{id}' not found");

            return bookQueryObject.MapToBook();
        }








        private async Task<Book> GetWithLinq(Guid id)
        {
            var query = _dbSet.AsQueryable();

            query = query.Include(a => a.Product)
                    .ThenInclude(a => a.Reviews)
                .Include(a => a.Product)
                    .ThenInclude(a => a.Product_Discounts)
                        .ThenInclude(a => a.Discount)
                .Include(a => a.Product)
                    .ThenInclude(a => a.Categories)
                .Include(a => a.Publisher)
                .Include(a => a.Translator)
                .Include(a => a.Authors);

            Book? book = await query.FirstOrDefaultAsync(a => a.Id == id);

            if (book == null)
                throw new NotFoundException($"Book with id '{id}' not found");

            return book;
        }








    }
}
