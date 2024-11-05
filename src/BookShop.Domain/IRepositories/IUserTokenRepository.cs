using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface IUserTokenRepository :
        IRepository,
        IReadRepository<UserToken, Guid>,
        IWriteRepository<UserToken, Guid>,
        IDeleteRepository<UserToken, Guid>
    {

        Task<UserToken> GetAsync(string name, string loginProvider);

        Task<UserToken?> GetOrDefaultAsync(string name, string loginProvider);




    }


}
