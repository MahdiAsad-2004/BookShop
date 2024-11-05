using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class RoleRepository : CrudRepository<Role, Guid>, IRoleRepository
    {
        public RoleRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {}


        public async Task<Role> GetByNameAsync(string name)
        {
            Role? role = await _dbSet.FirstOrDefaultAsync(a => a.Name == name);

            if (role == null)
                throw new NotFoundException($"Role with Name ({name}) not found");

            return role;
        }

     
        public async Task<Role> GetByNormalizedNameOrDefaultAsync(string normalizedName)
        {
            Role? role = await _dbSet.FirstOrDefaultAsync(a => a.NormalizedName == normalizedName);
            return role;
        }


    }
}
