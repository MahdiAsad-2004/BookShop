
using BookShop.Infrastructure.Persistance;
using BookShop.Infrastructure.Persistance.SeedDatas;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Infrstructure.Persistance.SeedDatas
{
    public class Seed
    {
        private readonly IServiceProvider _serviceProvider;
        public Seed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public async Task SeedDatas()
        {
            using var scope = _serviceProvider.CreateScope();    
            var dbContext = scope.ServiceProvider.GetService<BookShopDbContext>();
            if(dbContext != null) 
            {
                if(await dbContext.Database.CanConnectAsync())
                {
                    if(dbContext.Users.Any() == false)
                    await dbContext.Users.AddAsync(UsersSeed.SuperAdmin);
                    await dbContext.SaveChangesAsync();
                }
            }


        }


    }
}
