using BookShop.Infrastructure.Persistance;
using BookShop.Infrstructure.Persistance.SeedDatas;
using BookShop.IntegrationTest.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace BookShop.IntegrationTest.Fixture
{
    public class WebAppFactoryFixture : IDisposable
    {
        public readonly Respawner Respawner;
        public readonly TestCache TestCache;
        public readonly TestRepository TestRepository;
        public readonly TestRequestHandler TestRequestHandler;
        private readonly TestWebApplicationFactory _webApplicationFactory;
        public readonly IServiceScopeFactory ServiceScopeFactory;
        //Before All Tests
        public WebAppFactoryFixture()
        {
            string connectionString = ConnectionStringProvider.Get();
            _webApplicationFactory = new TestWebApplicationFactory(connectionString);
            //ServiceScopeFactory = ServiceScopeFactory;
            ServiceScopeFactory = _webApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();
            TestRequestHandler = new TestRequestHandler(ServiceScopeFactory);
            TestRepository = new TestRepository(ServiceScopeFactory);
            TestCache = new TestCache(ServiceScopeFactory);
            
            CreateDatabase().GetAwaiter().GetResult();

            Respawner = Respawner.CreateAsync(connectionString, new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory", "Permissions", "Users" , "Roles"},
            }).GetAwaiter().GetResult();

            
        }

        //After All Tests 
        public async void Dispose()
        {
            _webApplicationFactory.Dispose();
        }


        private async Task CreateDatabase()
        {
            using var scope = ServiceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();

            if (await dbContext.Database.CanConnectAsync() == false)
            {
                await dbContext.Database.MigrateAsync();
            }
            if (await dbContext.Database.CanConnectAsync())
            {
                // add user
                if (await dbContext.Users.AnyAsync(a => a.Id == TestCurrentUser.CurrentUserId) == false)
                {
                    await dbContext.Users.AddAsync(TestCurrentUser.User);
                    await dbContext.SaveChangesAsync();
                }

                // add permissions
                if (await dbContext.Permissions.AnyAsync() == false)
                {
                    var permissions = PermissionsSeed.GetPermissions(TestCurrentUser.CurrentUserId);
                    await dbContext.Permissions.AddRangeAsync(permissions);
                    await dbContext.SaveChangesAsync();
                }
            }
        }










    }
}
