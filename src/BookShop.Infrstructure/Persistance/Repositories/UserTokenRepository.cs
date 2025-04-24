using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class RefreshTokenRepository : CrudRepository<RefreshToken, Guid> , IRefreshTokenRepository
    {
        public RefreshTokenRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }

        public async Task<RefreshToken?> GetIfValid(string token, RefreshTokenQueryOption? queryOption = null)
        {
            var query = _dbSet.AsNoTracking();
            if(queryOption != null)
            {
                if (queryOption.IncludeUser)
                {
                    query = query.Include(a => a.User);
                }
            }
            return await query.FirstOrDefaultAsync(a => a.TokenValue == token && a.ExpiredAt > DateTime.UtcNow && a.Revoked == false);
        }


        //public async Task<UserToken> GetAsync(string name , string loginProvider)
        //{
        //    UserToken? userToken = await _dbSet
        //        .FirstOrDefaultAsync(a => a.TokenName == name && a.LoginProvider == loginProvider);

        //    if (userToken == null)
        //        throw new NotFoundException($"UserToken with Name ({name}) and LoginProvider ({loginProvider}) not found");

        //    return userToken;
        //}


        //public async Task<UserToken?> GetOrDefaultAsync(string name, string loginProvider)
        //{
        //    UserToken? userToken = await _dbSet
        //        .FirstOrDefaultAsync(a => a.TokenName == name && a.LoginProvider == loginProvider);

        //    return userToken;
        //}


    }
}
