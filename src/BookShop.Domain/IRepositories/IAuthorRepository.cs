using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IAuthorRepository :
        IRepository,
        IReadRepository<Author, Guid>,
        IWriteRepository<Author, Guid>,
        IDeleteRepository<Author, Guid>
    {


        Task<bool> IsExist(string name);

        Task<bool> IsExist(string name, Guid exceptId);

        Task<bool> AreExist(Guid[] ids);


    }
}
