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



    }
}
