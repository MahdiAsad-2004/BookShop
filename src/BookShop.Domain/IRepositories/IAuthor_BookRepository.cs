using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IAuthor_BookRepository :
        IRepository
    {

        Task InsertNewOnesAndDeleteAnothers(Guid bookId, Guid[] authorIds);  



    }
}
