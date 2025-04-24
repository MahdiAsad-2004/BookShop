using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.IntegrationTest.Common
{
    public class TestCurrentUserSetting
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public TestCurrentUserSetting(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        public void SetCurrentUser()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                CurrentUserCacheObject currentUserCacheObject = new CurrentUserCacheObject
                {
                    Authenticated = true,
                    Email = TestCurrentUser.User.Email,
                    Id = TestCurrentUser.User.Id,
                    Name = TestCurrentUser.User.Name,
                    PhoneNumber = TestCurrentUser.User.PhoneNumber,
                    Username = TestCurrentUser.User.Username
                };
                IMemoryCache memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                memoryCache.Set(TestCurrentUser.CurrentUserCachKey, currentUserCacheObject, DateTimeOffset.UtcNow.AddMinutes(1));
            }
        }

        public async Task SetCurrentUserAnonymous()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IMemoryCache memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                memoryCache.Remove(TestCurrentUser.CurrentUserCachKey);
            }
        }



    }
}
