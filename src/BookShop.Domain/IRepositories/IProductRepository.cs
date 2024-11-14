using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;
using BookShop.Domain.QueryOptions;

namespace BookShop.Domain.IRepositories
{
    public interface IProductRepository :
        IRepository,
        IReadRepository<Product, Guid>,
        IWriteRepository<Product, Guid>,
        IDeleteRepository<Product, Guid>
    {

        public Task<Product> GetByTitle(string title , ProductQueryOption? queryOption = null);
     

        public Task<Product> Get(Guid id, ProductQueryOption? queryOption = null);


    }

}
