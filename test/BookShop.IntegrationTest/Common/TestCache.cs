
using BookShop.Application.Caching;
using BookShop.Application.Common.Request;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.IntegrationTest.Common
{
    public class TestCache
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public TestCache(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        public async Task<TResponse?> GetFromCache<TRequest, TResponse>(TRequest request)
          where TRequest : CachableRequest<TResponse>
          where TResponse : class
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var x = request.GetCacheKey();
                ICache cache = scope.ServiceProvider.GetRequiredService<ICache>();
                TResponse? response = cache.GetOrDefault<TResponse>(request.GetCacheKey());
                return response;
            }
        }

        
        public void RemoveUserPermissionsCache()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                ICache cache = scope.ServiceProvider.GetRequiredService<ICache>();
                cache.Remove(CacheConstants.UserPermissions(TestCurrentUser.CurrentUserId));            
            }
        }







    }
}
