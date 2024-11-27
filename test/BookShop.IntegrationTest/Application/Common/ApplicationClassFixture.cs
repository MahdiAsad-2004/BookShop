using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace BookShop.IntegrationTest.Application.Common
{
    public class ApplicationClassFixture : IDisposable
    {
        internal readonly TestDbContext TestDbContext;
        internal readonly IMediator Mediator;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Respawner _respawner;
        //public ApplicationClassFixture(IMediator mediator, IMemoryCache memoryCache)
        //{
        //    //_respawner = applicationCollectionFixture._respawner;
        //    Mediator = mediator;
        //    _memoryCache = memoryCache;
        //    //_serviceScopeFactory = serviceScopeFactory;
        //    TestDbContext = new TestDbContext(_serviceScopeFactory);
        //}

        public ApplicationClassFixture(ApplicationCollectionFixture applicationCollectionFixture)
        {
            var x = applicationCollectionFixture;

            //var a = applicationCollectionFixture

        }


        private async Task AfterEachTest()
        {

        }

        private async Task BeforeEachTest()
        {
            //await _respawner.ResetAsync(ConnectionString);
        }

        public static string ConnectionString
        {
            get { return GetConectionString(); }
        }

        private static string GetConectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                //.AddEnvironmentVariables()
                .Build();

            string path = "ConnectionStrings:TestDatabaseConnectionString";

            string? connectionString = configuration.GetValue<string>(path);

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception($"Connection string in path {path} not found");

            return connectionString;
        }

        public async void Dispose()
        {
            await AfterEachTest();
        }


        public async Task SetCurrentUser(CurrentUserCacheObject currentUserCacheObject)
        {
            _memoryCache.Set(TestCurrentUser.CurrentUserCachKey, currentUserCacheObject, DateTimeOffset.UtcNow.AddMinutes(3));
        }


        public async Task SetCurrentUserAnonymous()
        {
            _memoryCache.Remove(TestCurrentUser.CurrentUserCachKey);
        }

    }
}
