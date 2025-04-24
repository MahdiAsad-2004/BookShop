using BookShop.Application.Caching;
using BookShop.Domain.Common.Event;
using BookShop.Domain.Constants;
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
        private readonly ICache _cache;
        public PermissionRepository(BookShopDbContext dbContext, ICurrentUser currentUser, IDomainEventPublisher domainEventPublisher, ICache cache)
            : base(dbContext, currentUser, domainEventPublisher)
        {
            _cache = cache;
        }



        public async Task<Permission[]> GetUserPermissions(Guid userId, string? roleName = null)
        {
            string userPermissionsCacheKey = CacheConstants.UserPermissions(userId);
            
            Permission[]? permissions = _cache.GetOrDefault<Permission[]>(userPermissionsCacheKey);

            if (permissions == null)
            {
                permissions = await _dbSet.AsNoTracking()
                    .Include(a => a.User_Permissions)
                    .SelectMany(a => a.User_Permissions)
                    .Where(a => a.UserId == userId)
                    .Select(a => a.Permission)
                    .ToArrayAsync();

                _cache.Add(userPermissionsCacheKey, permissions, TimeSpan.FromDays(10));
            }
            return permissions;

        }


    }
}
