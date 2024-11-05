using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;
using BookShop.Domain.QueryOptions;

namespace BookShop.Domain.IRepositories
{
    public interface IUserRepository:
        IRepository,
        IReadRepository<User, Guid ,UserQueryOption>,
        IWriteRepository<User, Guid>,
        IDeleteRepository<User, Guid>
    {

        Task<User> GetByNormalizedEmail(string normalizedEmail);
        
        Task<User> GetByNormalizedUsername(string normalizedUsername);

        Task<User?> GetByNormalizedUsernameOrDefault(string normalizedUsername);




    }




}
