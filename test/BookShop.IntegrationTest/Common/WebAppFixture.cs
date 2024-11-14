using BookShop.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace BookShop.IntegrationTest.Common
{
    public class WebAppFixture : IDisposable
    {
        private readonly string _connectionString;
        private readonly TestWebApplicationFactory _webApplicationFactory;
        public readonly IServiceScopeFactory _serviceScopeFactory;
        public readonly Respawner _respawner;

        public WebAppFixture()
        {
            _connectionString = TestBase.ConnectionString;
            _webApplicationFactory = new TestWebApplicationFactory(_connectionString);
            _serviceScopeFactory = _webApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();
            
            CreateDatabase().GetAwaiter().GetResult();
            
            _respawner = Respawner.CreateAsync(_connectionString, new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            }).GetAwaiter().GetResult();
        }

      


        private async Task CreateDatabase()
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<BookShopDbContext>();
            
            if (await dbContext.Database.CanConnectAsync() == false)
            {
                await dbContext.Database.MigrateAsync();
            }
        }




        public async void Dispose()
        {
            await _respawner.ResetAsync(_connectionString);
        }

    }
}
