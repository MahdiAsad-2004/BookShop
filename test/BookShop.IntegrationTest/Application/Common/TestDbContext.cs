
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.IntegrationTest.Application.Common
{
    public class TestDbContext
    {
        private BookShopDbContext _bookShopDbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public TestDbContext(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }



        public async Task<TEntity?> Get<TEntity,TKey>(TKey id) where TEntity : Entity<TKey>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _bookShopDbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            TEntity? entity = await _bookShopDbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(a => a.Id.Equals(id));
            await _bookShopDbContext.DisposeAsync();
            scope.Dispose();
            return entity;
        }

        
        public async Task<List<TEntity>> GetAll<TEntity,TKey>(Func<TEntity,bool> predicate) where TEntity : Entity<TKey>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _bookShopDbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            List<TEntity> entities = _bookShopDbContext.Set<TEntity>().AsNoTracking().Where(predicate).ToList();
            await _bookShopDbContext.DisposeAsync();
            scope.Dispose();
            return entities;
        }

        
        public async Task<int> Count<TEntity,TKey>() where TEntity : Entity<TKey>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _bookShopDbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            int count = await _bookShopDbContext.Set<TEntity>().AsNoTracking().CountAsync();
            await _bookShopDbContext.DisposeAsync();
            scope.Dispose();
            return count;
        }


        public async Task Add<TEntity, TKey>(TEntity entity) where TEntity : Entity<TKey>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _bookShopDbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            await _bookShopDbContext.AddAsync(entity);
            await _bookShopDbContext.SaveChangesAsync();
            _bookShopDbContext.ChangeTracker.Clear();
            await _bookShopDbContext.DisposeAsync();
            scope.Dispose();
        }


        public async Task Add<TEntity, TKey>(List<TEntity> entities) where TEntity : Entity<TKey>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _bookShopDbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            await _bookShopDbContext.AddRangeAsync(entities);
            await _bookShopDbContext.SaveChangesAsync();
            _bookShopDbContext.ChangeTracker.Clear();
            await _bookShopDbContext.DisposeAsync();
            scope.Dispose();
        }


        public async Task SetPermissionForUser(string permissionName)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _bookShopDbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            var a = _bookShopDbContext.Permissions.Count();
            Permission permission = await _bookShopDbContext.Permissions.FirstAsync(a => a.Name == permissionName);
            await _bookShopDbContext.User_Permissions.AddAsync(new User_Permission
            {
                CreateDate = DateTime.Now,
                CreateBy = TestCurrentUser.CurrentUserId.ToString(),
                Id = Guid.NewGuid(),
                LastModifiedBy = TestCurrentUser.CurrentUserId.ToString(),
                LastModifiedDate = DateTime.Now,
                IsDeleted = false,
                DeleteDate = null,
                DeletedBy = null,
                UserId = TestCurrentUser.CurrentUserId,
                PermissionId = permission.Id,
            });
            await _bookShopDbContext.SaveChangesAsync();
            await _bookShopDbContext.DisposeAsync();
            scope.Dispose();
        }



        public async Task SetPermissionForUser(params string[] permissionNames)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _bookShopDbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            foreach (string permissionName in permissionNames)
            {
                Permission permission = await _bookShopDbContext.Permissions.FirstAsync(a => a.Name == permissionName);
                await _bookShopDbContext.User_Permissions.AddAsync(new User_Permission
                {
                    CreateDate = DateTime.Now,
                    CreateBy = TestCurrentUser.CurrentUserId.ToString(),
                    Id = Guid.NewGuid(),
                    LastModifiedBy = TestCurrentUser.CurrentUserId.ToString(),
                    LastModifiedDate = DateTime.Now,
                    IsDeleted = false,
                    DeleteDate = null,
                    DeletedBy = null,
                    UserId = TestCurrentUser.CurrentUserId,
                    PermissionId = permission.Id,
                });
            }
            await _bookShopDbContext.SaveChangesAsync();
            await _bookShopDbContext.DisposeAsync();
            scope.Dispose();
        }



        public async Task ClearUsersExceptTestUser()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _bookShopDbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            await _bookShopDbContext.Users.Where(a => a.Username != TestCurrentUser.User.Username).ExecuteDeleteAsync();
            await _bookShopDbContext.SaveChangesAsync();
            await _bookShopDbContext.DisposeAsync();
            scope.Dispose();
        }



    }
}
