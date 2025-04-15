using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;
using BookShop.Domain.QueryOptions;

namespace BookShop.Domain.IRepositories
{
    public interface IEBookRepository : IRepository,
        IReadRepository<EBook, Guid>,
        IWriteRepository<EBook, Guid>,
        IDeleteRepository<EBook, Guid>
    {
        Task Add(EBook ebook, Product product, Guid[] authorIds);

        Task<EBook> Get(Guid id, EBookQueryOption queryOption);




    }
}
