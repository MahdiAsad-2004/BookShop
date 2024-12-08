using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;
using BookShop.Domain.QueryOptions;

namespace BookShop.Domain.IRepositories
{
    public interface IBookRepository :
        IRepository,
        IReadRepository<Book, Guid>,
        IWriteRepository<Book, Guid>,
        IDeleteRepository<Book, Guid>
    {

        public Task<Book> Get(Guid id, BookQueryOption queryOption);

        public Task<PaginatedEntities<Book>> GetAll(BookQueryOption queryOption, Paging? paging = null, BookSortingOrder? sortingOrder = null);


    }
}
