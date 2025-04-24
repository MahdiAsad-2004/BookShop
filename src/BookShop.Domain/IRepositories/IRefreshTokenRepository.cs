using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;
using BookShop.Domain.QueryOptions;

namespace BookShop.Domain.IRepositories
{
    public interface IRefreshTokenRepository :
        IRepository,
        IReadRepository<RefreshToken, Guid>,
        IWriteRepository<RefreshToken, Guid>,
        IDeleteRepository<RefreshToken, Guid>
    {

        //Task<UserToken> GetAsync(string name, string loginProvider);

        //Task<UserToken?> GetOrDefaultAsync(string name, string loginProvider);


        Task<RefreshToken?> GetIfValid(string tokenValue , RefreshTokenQueryOption? queryOption = null);
        



    }


}
