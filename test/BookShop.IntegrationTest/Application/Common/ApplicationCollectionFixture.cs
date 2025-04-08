using BookShop.Domain.Identity;
using BookShop.Infrastructure.Persistance;
using BookShop.Infrastructure.Persistance.SeedDatas;
using BookShop.Infrstructure.Persistance.SeedDatas;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace BookShop.IntegrationTest.Application.Common
{
    public class ApplicationCollectionFixture : IDisposable
    {
        private readonly string _connectionString;
        private readonly TestWebApplicationFactory _webApplicationFactory;
        public readonly IServiceScopeFactory _serviceScopeFactory;
        public readonly Respawner _respawner;
        public ApplicationCollectionFixture()
        {
            _connectionString = TestBase.ConnectionString;
            _webApplicationFactory = new TestWebApplicationFactory(_connectionString);
            _serviceScopeFactory = _webApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();

            CreateDatabase().GetAwaiter().GetResult();

            _respawner = Respawner.CreateAsync(_connectionString, new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory", "Permissions", "Users" },
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




        public async void Dispose()
        {
            _webApplicationFactory.Dispose();
        }






    }
}
