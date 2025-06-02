using Bogus;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Infrastructure.Persistance.SeedDatas;

namespace BookShop.Infrstructure.Persistance.SeedDatas
{
    internal class BooksSeed
    {
        private int counter = 0;
        public readonly int Counts = 50;
        private readonly string userId;
        private readonly Guid[] authorIds;
        private readonly Guid[] publisherIds;
        private readonly Guid[] categoryIds;
        private readonly Guid[] translatorIds;
        private readonly DiscountSeed _discountSeed;
        private readonly Randomizer _randomizer = new Randomizer();
        public Faker<Book> Faker = new Faker<Book>();

        public BooksSeed(string userId, Guid[] publisherIds, Guid[] categoryIds, Guid[] authorIds, Guid[] translatorIds)
        {
            this.userId = userId;
            this.publisherIds = publisherIds;
            this.categoryIds = categoryIds;
            this.authorIds = authorIds;
            _discountSeed = new DiscountSeed(userId);
            this.translatorIds = translatorIds;
        }

        public List<Book> GetBooks()
        {
            List<Book> books = new List<Book>();
            for (int i = 1; i <= Counts; i++)
            {
                books.Add(CreateRandomBook());
            }
            return books;
        }


        private Book CreateRandomBook()
        {
            counter++;
            DateTime now = DateTime.UtcNow;
            int discountCount = _randomizer.WeightedRandom([0, 1, 2, 3], [0.2f, 0.2f, 0.3f, 0.3f]);
            Guid productId = Guid.NewGuid();
            var book = new Book
            {
                Cover = _randomizer.Enum<Cover>(),
                CreateBy = UsersSeed.SuperAdminId.ToString(),
                CreateDate = now,
                DeleteDate = null,
                Cutting = _randomizer.Enum<Cutting>(),
                DeletedBy = null,
                Edition = _randomizer.Int(1, 10),
                Id = productId,
                IsDeleted = false,
                Language = _randomizer.Enum<Language>(),
                LastModifiedBy = UsersSeed.SuperAdminId.ToString(),
                LastModifiedDate = now,
                NumberOfPages = _randomizer.Int(10, 1000),
                Shabak = string.Empty,
                WeightInGram = 100,
                Product = new Product
                {
                    DescriptionHtml = string.Empty,
                    ImageName = $"book-{counter}.jpg",
                    Price = _randomizer.Int(100_000, 1_000_000),
                    NumberOfInventory = _randomizer.Bool(0.8f) ? _randomizer.Int(1, 500) : 0,
                    ProductType = ProductType.Book,
                    SellCount = _randomizer.Int(0, 100),
                    Title = $"Product - Book - {counter}",
                    CreateBy = userId,
                    CreateDate = now,
                    DeleteDate = null,
                    DeletedBy = null,
                    Id = productId,
                    IsDeleted = false,
                    LastModifiedBy = userId,
                    LastModifiedDate = now,
                    CategoryId = _randomizer.Bool(0.8f) ? categoryIds[Random.Shared.Next(0, categoryIds.Length)] : null,
                    Product_Discounts = _discountSeed.CreateRandom(discountCount)
                },
                PublishYear = DateTime.UtcNow,
                PublisherId = publisherIds[Random.Shared.Next(0, publisherIds.Length)],
                Author_Books = new List<Author_Book>(),
                TranslatorId = _randomizer.Bool(0.2f) ? _randomizer.ArrayElement(translatorIds) : null, 
            };
            Author_Book author_book;
            int authorsCount = _randomizer.WeightedRandom([1, 2, 3], [0.5f, 0.3f, 0.2f]);
            for (int i = 1; i <= authorsCount; i++)
            {
                author_book = AuthorSeed.RandomAuthor_Book(userId);
                author_book.AuthorId = authorIds[Random.Shared.Next(0, authorIds.Length)];
                book.Author_Books.Add(author_book);
            }
            return book;
        }


    }
}
