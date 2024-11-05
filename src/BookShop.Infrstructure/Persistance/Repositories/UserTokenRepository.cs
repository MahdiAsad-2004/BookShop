using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class UserTokenRepository : CrudRepository<UserToken, Guid> , IUserTokenRepository
    {
        public UserTokenRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {
        }


        public async Task<UserToken> GetAsync(string name , string loginProvider)
        {
            UserToken? userToken = await _dbSet
                .FirstOrDefaultAsync(a => a.TokenName == name && a.LoginProvider == loginProvider);
            
            if (userToken == null)
                throw new NotFoundException($"UserToken with Name ({name}) and LoginProvider ({loginProvider}) not found");
            
            return userToken;
        }


        public async Task<UserToken?> GetOrDefaultAsync(string name, string loginProvider)
        {
            UserToken? userToken = await _dbSet
                .FirstOrDefaultAsync(a => a.TokenName == name && a.LoginProvider == loginProvider);

            return userToken;
        }


    }
}
