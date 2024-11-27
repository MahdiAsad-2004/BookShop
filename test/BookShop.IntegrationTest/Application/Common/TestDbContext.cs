
using BookShop.Domain.Common.Entity;
using BookShop.Infrastructure.Persistance;
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


        public async Task Add<TEntity,TKey>(TEntity entity) where TEntity : Entity<TKey>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _bookShopDbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            await _bookShopDbContext.AddAsync(entity);
            await _bookShopDbContext.SaveChangesAsync();
            await _bookShopDbContext.DisposeAsync();
            scope.Dispose();
        }

        
        public async Task Add<TEntity,TKey>(List<TEntity> entities) where TEntity : Entity<TKey>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _bookShopDbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            await _bookShopDbContext.AddRangeAsync(entities);
            await _bookShopDbContext.SaveChangesAsync();
            await _bookShopDbContext.DisposeAsync();
            scope.Dispose();
        }



    }
}
