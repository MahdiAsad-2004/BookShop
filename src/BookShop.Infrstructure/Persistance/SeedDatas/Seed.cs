
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;
using BookShop.Infrastructure.Persistance;
using BookShop.Infrastructure.Persistance.SeedDatas;
using Microsoft.EntityFrameworkCore;
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


        public async Task MigrateDatabaseIfNotExist()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<BookShopDbContext>();
            if (dbContext != null)
            {
                if (await dbContext.Database.CanConnectAsync() == false)
                {
                    await dbContext.Database.MigrateAsync();
                }
            }
        }

        public async Task SeedDatas()
        {
            string userId = UsersSeed.SuperAdmin.Id.ToString();
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<BookShopDbContext>();
            IPasswordHasher passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            if (dbContext != null)
            {
                if (await dbContext.Database.CanConnectAsync())
                {
                    if (dbContext.Users.Any() == false)
                    {
                        await dbContext.Users.AddAsync(UsersSeed.SuperAdmin);
                        await dbContext.SaveChangesAsync();
                        UsersSeed usersSeed = new UsersSeed(passwordHasher);
                        await dbContext.Users.AddRangeAsync(usersSeed.Get());
                        await dbContext.SaveChangesAsync();
                    }
                    if (dbContext.Permissions.Any() == false)
                    {
                        await dbContext.Permissions.AddRangeAsync(PermissionsSeed.GetPermissions(UsersSeed.SuperAdmin.Id));
                        await dbContext.SaveChangesAsync();
                    }
                    if(dbContext.Publishers.Any() == false)
                    {
                        List<Publisher> publishers = new PublishersSeed(userId).GetPublishers();
                        await dbContext.Publishers.AddRangeAsync(publishers);
                        await dbContext.SaveChangesAsync();
                    }
                    if(dbContext.Authors.Any() == false)
                    {
                        List<Author> authors = new AuthorSeed(userId).Get();
                        await dbContext.Authors.AddRangeAsync(authors);
                        await dbContext.SaveChangesAsync();
                    }
                    if(dbContext.Translators.Any() == false)
                    {
                        List<Translator> translators = new TranslatorsSeed(userId).Get();
                        await dbContext.Translators.AddRangeAsync(translators);
                        await dbContext.SaveChangesAsync();
                    }
                    if(dbContext.Categories.Any() == false)
                    {
                        List<Category> categories = new CategorySeed(userId).GetCategories();
                        await dbContext.Categories.AddRangeAsync(categories);
                        await dbContext.SaveChangesAsync();
                    }
                    if(dbContext.Books.Any() == false)
                    {
                        Guid[] publisherIds = dbContext.Publishers.Select(a => a.Id).ToArray();
                        Guid[] categoryIds = dbContext.Categories.Select(a => a.Id).ToArray();
                        Guid[] authorIds = dbContext.Authors.Select(a => a.Id).ToArray();
                        Guid[] translatorIds = dbContext.Translators.Select(a => a.Id).ToArray();
                        List<Book> books = new BooksSeed(userId, publisherIds, categoryIds , authorIds,translatorIds).GetBooks();
                        await dbContext.Books.AddRangeAsync(books);
                        await dbContext.SaveChangesAsync();
                    }
                    if (dbContext.Reviews.Any() == false) 
                    {
                        Guid[] userIds = await dbContext.Users.Select(a => a.Id).ToArrayAsync();
                        Guid[] productIds = await dbContext.Products.Select(a => a.Id).ToArrayAsync();
                        List<Review> reviews = new ReviewSeed(userIds, productIds).Get();
                        await dbContext.Reviews.AddRangeAsync(reviews);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }


        }


    }
}
