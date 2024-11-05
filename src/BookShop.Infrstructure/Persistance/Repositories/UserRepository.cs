using BookShop.Domain.Common.Event;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Domain.QueryOptions;

using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class UserRepository : CrudRepository<User, Guid,UserQueryOption> , IUserRepository
    {
        public UserRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {}


        public async Task<User> GetByNormalizedUsername(string normalizedUsername)
        {
            User? user = await _dbSet.FirstOrDefaultAsync(a => a.NormalizedUsername == normalizedUsername);
            if (user == null)
                throw new NotFoundException($"User with NormalizedUsername ({normalizedUsername}) not found");
            return user;
        }


        public async Task<User> GetByNormalizedEmail(string normalizedEmail)
        {
            User? user = await _dbSet.FirstOrDefaultAsync(a => a.NormalizedEmail == normalizedEmail);
            if (user == null)
                throw new NotFoundException($"User with NormalizedEmail ({normalizedEmail}) not found");
            return user;
        }

        public async Task<User?> GetByNormalizedUsernameOrDefault(string normalizedUsername)
        {
            User? user = await _dbSet.FirstOrDefaultAsync(a => a.NormalizedUsername == normalizedUsername);
            return user;
        }

        public override Task<IEnumerable<User>> GetAll(Action<UserQueryOption> queryOptionConfig)
        {
            var query = _dbSet.AsQueryable();
            UserQueryOption userQueryOption = _queryOptionOperator.ConfigureQueryOption(queryOptionConfig);
            query = _queryOptionOperator.PerformEntityIncludes(userQueryOption,query);

            return Task.FromResult(query.AsEnumerable());
        }

        public override async Task<User> Get(Guid key, Action<UserQueryOption> queryOptionConfig)
        {
            var query = _dbSet.AsQueryable();
            UserQueryOption userQueryOption = _queryOptionOperator.ConfigureQueryOption(queryOptionConfig);
            query = _queryOptionOperator.PerformEntityIncludes(userQueryOption, query);
       
            User? user = await query.FirstOrDefaultAsync(a => a.Id == key);
       
            if (user == null)
                throw new NotFoundException($"User with id ({key} not found)");
        
            return user;
        }
    }

    

}
