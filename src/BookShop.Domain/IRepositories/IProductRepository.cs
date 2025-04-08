using BookShop.Domain.Common.Entity;
using BookShop.Domain.Common.QueryOption;
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
     

        //public Task<Product> Get(Guid id, ProductQueryOption queryOption);

        
        //public Task<IEnumerable<Product>> GetAll(ProductQueryOption queryOption, Paging? paging = null, ProductSortingOrder? sortingOrder = null );


        public Task<Product> GetWithQuery(Guid id, ProductQueryOption? queryOption);


        public Task<PaginatedEntities<Product>> GetAllWithQuery(ProductQueryOption queryOption, Paging? paging = null, ProductSortingOrder? sortingOrder = null);


        public Task<bool> IsExist(string title);
    
        public Task<bool> IsExist(string title,Guid exceptId);


    }

}
