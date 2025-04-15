using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IAuthor_EBookRepository :
        IRepository
    {

        Task InsertNewOnesAndDeleteAnothers(Guid ebookId, Guid[] authorIds);  



    }
}
