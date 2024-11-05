using BookShop.Domain.Common.Event;
using BookShop.Domain.Entities;
using BookShop.Domain.Exceptions;
using BookShop.Domain.Identity;
using BookShop.Domain.IRepositories;
using BookShop.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BookShop.Infrastructure.Persistance.Repositories
{
    internal class PermissionRepository : CrudRepository<Permission, Guid>, IPermissionRepository
    {
        public PermissionRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher)
            : base(dbContext, currentUser, domainEventPublisher)
        {}



        public Task<IEnumerable<Permission>> GetUserPermissions(Guid userId)
        {
            IEnumerable<Permission> permissions = _dbSet
                .Include(a => a.User_Permissions)
                .SelectMany(a => a.User_Permissions)
                .Where(a => a.UserId == userId)
                .Select(a => a.Permission)
                .AsEnumerable();

            return Task.FromResult(permissions);
        }


      
        
    }
}
