global using E = BookShop.Domain.Entities;
using Bogus;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;


namespace BookShop.IntegrationTest.Application.Common
{
    [Collection("ApplicationCollectionFixture")]
    public class TestBase : IDisposable
    {
        internal readonly TestDbContext _TestDbContext;
        public readonly ApplicationCollectionFixture _applicationCollectionFixture;

        public TestBase(ApplicationCollectionFixture applicationCollectionFixture)
        {
            _applicationCollectionFixture = applicationCollectionFixture;
            _TestDbContext = new TestDbContext(_applicationCollectionFixture._serviceScopeFactory);
            BeforeEachTest().GetAwaiter().GetResult();
        }


        private async Task AfterEachTest()
        {
        }

        private async Task BeforeEachTest()
        {
            await _applicationCollectionFixture._respawner.ResetAsync(ConnectionString);
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



      
        public async Task SendRequest<TRequest>(TRequest request) where TRequest : IRequest
        {
            using (var scopr = _applicationCollectionFixture._serviceScopeFactory.CreateScope())
            {
                IMediator mediator = scopr.ServiceProvider.GetRequiredService<IMediator>(); 
                await mediator.Send(request);
            }
        }
        
        public async Task<TRessponse> SendRequest<TRequest,TRessponse>(TRequest request) where TRequest : IRequest<TRessponse>
        {
            using (var scopr = _applicationCollectionFixture._serviceScopeFactory.CreateScope())
            {
                IMediator mediator = scopr.ServiceProvider.GetRequiredService<IMediator>(); 
                TRessponse ressponse = await mediator.Send(request);
                return ressponse;
            }
        }


        public async Task SetCurrentUser(CurrentUserCacheObject currentUserCacheObject)
        {
            using (var scope = _applicationCollectionFixture._serviceScopeFactory.CreateScope())
            {
                currentUserCacheObject.Id = TestCurrentUser.CurrentUserId;
                IMemoryCache memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                memoryCache.Set(TestCurrentUser.CurrentUserCachKey, currentUserCacheObject, DateTimeOffset.UtcNow.AddMinutes(3));
            }
        }


        public async Task SetCurrentUserAnonymous()
        {
            using (var scope = _applicationCollectionFixture._serviceScopeFactory.CreateScope())
            {
                IMemoryCache memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                memoryCache.Remove(TestCurrentUser.CurrentUserCachKey);
            }
        }



    }
}
