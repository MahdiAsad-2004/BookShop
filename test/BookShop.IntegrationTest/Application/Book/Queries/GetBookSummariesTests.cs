
using Bogus;
using BookShop.Application.Common.Dtos;
using BookShop.Application.Features.Book.Dtos;
using BookShop.Application.Features.Book.Queries.GetSummaries;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Application.Features.Product.Queries.GetSummaries;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Domain.QueryOptions;
using BookShop.IntegrationTest.Application.Author.FakeData;
using BookShop.IntegrationTest.Application.Book.FakeData;
using BookShop.IntegrationTest.Application.Category.FakeData;
using BookShop.IntegrationTest.Application.Common;
using BookShop.IntegrationTest.Application.Product.FakeData;
using BookShop.IntegrationTest.Application.Publisher.FakeData;
using BookShop.IntegrationTest.Application.Translator.FakeData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Serilog;
using Xunit.Abstractions;

namespace BookShop.IntegrationTest.Application.Book.Queries
{
    public class GetBookSummariesTests : TestBase
    {
        public GetBookSummariesTests(ApplicationCollectionFixture applicationCollectionFixture,ITestOutputHelper testOutputHelper) 
            : base(applicationCollectionFixture,testOutputHelper)
        { }


        [Fact]
        public async Task ShouldReturn()
        {
            //Arrange
            List<E.Book> books = BookFakeData.CreateBetween(3, 5);
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {

            });

            //Assert
            Assert.Equal(books.Count, paginatedBookSummaries.Dtos.Count);
        }


        [Fact]
        public async Task WithSort_PublishYearAsce_ShouldReturn_Sorted()
        {
            //Arrange
            List<E.Book> books = BookFakeData.CreateBetween(3, 5);
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                SortingOrder = BookSortingOrder.PublishYearAsce
            });

            //Assert
            Assert.Equal(books.Count, paginatedBookSummaries.Dtos.Count);
            var sortedBooks = books.OrderBy(a => a.PublishYear).ToList();
            for (int i = 0; i < books.Count; i++)
            {
                Assert.Equal(sortedBooks[i].Id.ToString(), paginatedBookSummaries.Dtos[i].Id);
            }
        }


        [Fact]
        public async Task WithSort_PublishYearDesc_ShouldReturn_Sorted()
        {
            //Arrange
            List<E.Book> books = BookFakeData.CreateBetween(3, 5);
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                SortingOrder = BookSortingOrder.PublishYearDesc
            });

            //Assert
            Assert.Equal(books.Count, paginatedBookSummaries.Dtos.Count);
            var sortedBooks = books.OrderByDescending(a => a.PublishYear).ToList();
            for (int i = 0; i < books.Count; i++)
            {
                Assert.Equal(sortedBooks[i].Id.ToString(), paginatedBookSummaries.Dtos[i].Id);
            }
        }


        [Fact]
        public async Task WithFilter_StartPublishYear_ShouldReturn_Filtered()
        {
            //Arrange
            List<E.Book> books = new List<E.Book>();
            DateTime publishYear = DateTime.Now;
            DateTime startPublishYear = DateTime.Now.AddMonths(Random.Shared.Next(-4, 0)).AddDays(-10);
            for (int i = 1; i <= 5; i++)
            {
                publishYear = publishYear.AddMonths(-i);
                books.Add(BookFakeData.Create(publishYear: publishYear));
            }
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                StartPublishYear = startPublishYear,
            });

            //Assert
            var filteredBooks = books.Where(a => a.PublishYear >= startPublishYear);
            Assert.Equal(filteredBooks.Count(), paginatedBookSummaries.Dtos.Count);
        }


        [Fact]
        public async Task WithFilter_EndPublishYear_ShouldReturn_Filtered()
        {
            //Arrange
            List<E.Book> books = new List<E.Book>();
            DateTime publishYear = DateTime.Now;
            DateTime endPublishYear = DateTime.Now.AddMonths(Random.Shared.Next(-4, 0)).AddDays(+10);
            for (int i = 1; i <= 5; i++)
            {
                publishYear = publishYear.AddMonths(-i);
                books.Add(BookFakeData.Create(publishYear: publishYear));
            }
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                EndPublishYear = endPublishYear,
            });

            //Assert
            var filteredBooks = books.Where(a => a.PublishYear <= endPublishYear);
            Assert.Equal(filteredBooks.Count(), paginatedBookSummaries.Dtos.Count);

        }


        [Fact]
        public async Task WithFilter_CategoryId_ShouldReturn_Filtered()
        {
            //Arrange
            int numberOfCategories = 2;
            List<E.Category> categories = CategoryFakeData.CreateBetween(numberOfCategories, numberOfCategories);
            List<E.Book> books = new List<E.Book>();
            int categoryIndex = 0;
            E.Product product;
            for (int i = 1; i <= 5; i++)
            {
                categoryIndex = Random.Shared.Next(-1, numberOfCategories);
                product = ProductFakeData.Create(category: categoryIndex >= 0 ? categories[categoryIndex] : null);
                books.Add(BookFakeData.Create(product: product));
            }
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            Guid filterCategoryId = categories.First().Id;
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(
                new GetBookSummariesQuery
                {
                    CategoryId = filterCategoryId
                });

            //Assert
            List<E.Book> filteredCategories = books.Where(a => a.Product.CategoryId == filterCategoryId).ToList();
            Assert.Equal(filteredCategories.Count, paginatedBookSummaries.Dtos.Count);
        }


        [Fact]
        public async Task WithFilter_CategoryId_WithParentCatgory_ShouldReturn_Filtered()
        {
            //Arrange
            int numberOfCategories = 5;
            List<E.Category> parentCategories = CategoryFakeData.CreateBetween(2, 2);
            List<E.Category> categories = CategoryFakeData.CreateBetween(numberOfCategories, numberOfCategories);
            for (int i = 0; i < categories.Count; i++)
            {
                categories[i].Parent = parentCategories[i % 2 == 0 ? 0 : 1];
            }
            List<E.Book> books = new List<E.Book>();
            int categoryIndex = 0;
            E.Product product;
            for (int i = 1; i <= 6; i++)
            {
                categoryIndex = Random.Shared.Next(-1, numberOfCategories);
                product = ProductFakeData.Create(category: categoryIndex >= 0 ? categories[categoryIndex] : null);
                books.Add(BookFakeData.Create(product: product));
            }
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            Guid filterParentCategoryId = parentCategories[0].Id;
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(
                new GetBookSummariesQuery
                {
                    CategoryId = filterParentCategoryId,
                });

            //Assert
            List<Guid> allChildCategoryIds = categories.Where(a => a.Parent.Id == filterParentCategoryId).Select(a => a.Id).ToList();
            allChildCategoryIds.Add(filterParentCategoryId);
            List<E.Book> filteredCategories = books.Where(a => allChildCategoryIds.Contains(a.Product.CategoryId ?? Guid.Empty)).ToList();
            Assert.Equal(filteredCategories.Count, paginatedBookSummaries.Dtos.Count);
        }


        [Fact]
        public async Task WithFilter_Cover_ShoudReturn_Filtered()
        {
            //Arrange
            Randomizer randomizer = new Randomizer();
            //Cover cover = randomizer.Enum<Cover>();
            Cover cover = Cover.HardCover;
            List<E.Book> books = BookFakeData.CreateBetween(3, 8);
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                Cover = cover,
            });

            //Assert
            var filteredBooks = books.Where(a => a.Cover == cover);
            Assert.Equal(filteredBooks.Count(), paginatedBookSummaries.Dtos.Count);
        }


        [Fact]
        public async Task WithFilter_Language_ShoudReturn_Filtered()
        {
            //Arrange
            Randomizer randomizer = new Randomizer();
            Language language = randomizer.Enum<Language>();
            List<E.Book> books = BookFakeData.CreateBetween(3, 8);
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                Language = language,
            });

            //Assert
            var filteredBooks = books.Where(a => a.Language == language);
            Assert.Equal(filteredBooks.Count(), paginatedBookSummaries.Dtos.Count);
        }


        [Fact]
        public async Task WithFilter_Cutting_ShoudReturn_Filtered()
        {
            //Arrange
            Randomizer randomizer = new Randomizer();
            Cutting cutting = randomizer.Enum<Cutting>();
            List<E.Book> books = BookFakeData.CreateBetween(3, 8);
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                Cutting = cutting,
            });

            //Assert
            var filteredBooks = books.Where(a => a.Cutting == cutting);
            Assert.Equal(filteredBooks.Count(), paginatedBookSummaries.Dtos.Count);
        }


        [Fact]
        public async Task WithFilter_PublisherId_ShoudReturn_Filtered()
        {
            //Arrange
            var publishers = PublisherFakeData.CreateBetween(1, 3);
            List<E.Book> books = BookFakeData.CreateBetween(3, 8);
            int randoemIndex= 0;
            foreach (var book in books)
            {
                randoemIndex = Random.Shared.Next(-1, publishers.Count);
                if(randoemIndex > -1)
                {
                    book.Publisher = publishers[randoemIndex];
                }
                
            }
            await _TestDbContext.Add<E.Book, Guid>(books);
            Guid publisherId = publishers[Random.Shared.Next(0, publishers.Count)].Id;

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                PublisherId = publisherId,
            });

            //Assert
            var filteredBooks = books.Where(a => a.PublisherId == publisherId);
            Assert.Equal(filteredBooks.Count(), paginatedBookSummaries.Dtos.Count);
            _testOutputHelper.WriteLine($"Publisher books count: {paginatedBookSummaries.Dtos.Count}");
        }

        
        [Fact]
        public async Task WithFilter_TranslatorId_ShoudReturn_Filtered()
        {
            //Arrange
            var translators = TranslatorFakeData.CreateBetween(1, 3);
            List<E.Book> books = BookFakeData.CreateBetween(3, 8);
            int randoemIndex= 0;
            foreach (var book in books)
            {
                randoemIndex = Random.Shared.Next(-1, translators.Count);
                if(randoemIndex > -1)
                {
                    book.Translator = translators[randoemIndex];
                }
                
            }
            await _TestDbContext.Add<E.Book, Guid>(books);
            Guid translatorId = translators[Random.Shared.Next(0, translators.Count)].Id;

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                TranslatorId = translatorId,
            });

            //Assert
            var filteredBooks = books.Where(a => a.TranslatorId == translatorId);
            Assert.Equal(filteredBooks.Count(), paginatedBookSummaries.Dtos.Count);
            _testOutputHelper.WriteLine($"Translator books count: {paginatedBookSummaries.Dtos.Count}");
        }


        [Fact]
        public async Task WithFilter_AuthorId_ShoudReturn_Filtered()
        {
            //Arrange
            var authors = AuthorFakeData.CreateBetween(1, 3);
            List<E.Book> books = BookFakeData.CreateBetween(3, 8);
            int randoemCount = 0;
            foreach (var book in books)
            {
                book.Author_Books = new List<Author_Book>();
                randoemCount = Random.Shared.Next(0, authors.Count);
                for (int i = 0; i <= randoemCount; i++)
                {
                    book.Author_Books.Add(new Author_Book
                    {
                        Author = authors[i],
                    });
                }
            }
            await _TestDbContext.Add<E.Book, Guid>(books);
            var authorId = authors[Random.Shared.Next(0, authors.Count)].Id;

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                AuthorId = authorId
            });

            //Assert
            var filteredBooks = books.Where(a => a.Author_Books.Any(b => b.AuthorId == authorId));
            Assert.Equal(filteredBooks.Count(), paginatedBookSummaries.Dtos.Count);
            _testOutputHelper.WriteLine($"Author books count: {paginatedBookSummaries.Dtos.Count}");
        }


        [Fact]
        public async Task WithFilter_StartPrice_ShouldReturn_Filtered()
        {
            //Arrange
            var books = BookFakeData.CreateBetween(3, 8);
            int startPrice = Random.Shared.Next(100_000, 1_000_000);
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedDtos = await SendRequest<GetBookSummariesQuery,PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                StartPrice = startPrice
            });

            //Assert
            int expectedBooksCount = books.Where(a => a.Product.Price >= startPrice).Count();
            Assert.Equal(expectedBooksCount, paginatedDtos.Dtos.Count);
            _testOutputHelper.WriteLine($"{paginatedDtos.Dtos.Count} books with startPrce {startPrice:c}");
        }


        [Fact]
        public async Task WithFilter_EndPrice_ShouldReturn_Filtered()
        {
            //Arrange
            var books = BookFakeData.CreateBetween(3, 8);
            int endPrice = Random.Shared.Next(100_000, 1_000_000);
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedDtos = await SendRequest<GetBookSummariesQuery,PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                EndPrice = endPrice
            });

            //Assert
            int expectedBooksCount = books.Where(a => a.Product.Price <= endPrice).Count();
            Assert.Equal(expectedBooksCount, paginatedDtos.Dtos.Count);
            _testOutputHelper.WriteLine($"{paginatedDtos.Dtos.Count} books with endPrce {endPrice:c}");
        }

        
        [Fact]
        public async Task WithFilter_Title_ShouldReturn_Filtered()
        {
            //Arrange
            var books = BookFakeData.CreateBetween(3, 8);
            string titleFilter = books[Random.Shared.Next(0, books.Count)].Product.Title;
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedDtos = await SendRequest<GetBookSummariesQuery,PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                Title = titleFilter
            });

            //Assert
            int expectedBooksCount = books.Count(a => a.Product.Title == titleFilter);
            Assert.Equal(expectedBooksCount, paginatedDtos.Dtos.Count);
            _testOutputHelper.WriteLine($"{paginatedDtos.Dtos.Count} books with title '{titleFilter}'");
        }


        [Fact]
        public async Task WithFilter_IsAvailable_ShouldReturn_Filtered()
        {
            //Arrange
            List<E.Book> books = new();
            for (int i = 0; i < 5; i++)
            {
                bool availableChance = Random.Shared.Next(0, 3) == 0;
                books.Add(BookFakeData.Create(product: ProductFakeData.Create(available: availableChance)));
            }
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedDtos = await SendRequest<GetBookSummariesQuery,PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                IsAvailable = true
            });

            //Assert
            int expectedBooksCount = books.Count(a => a.Product.NumberOfInventory > 0);
            Assert.Equal(expectedBooksCount, paginatedDtos.Dtos.Count);
            _testOutputHelper.WriteLine($"{paginatedDtos.Dtos.Count} books are available from {books.Count}");
        }


        [Fact]
        public async Task WithFilter_AcceptedAverageScore_ShouldReturn_Filtered()
        {
            //Arange
            byte averageScoreFilter = 3;
            var reviewFaker = new Faker<E.Review>();
            reviewFaker.RuleFor(r => r.Score, (f, s) => f.Random.Byte(1, 5));
            reviewFaker.RuleFor(r => r.IsAccepted, (f, a) => f.Random.Bool(0.7f));
            reviewFaker.RuleFor(r => r.Id, (f, a) => Guid.NewGuid());
            reviewFaker.RuleFor(r => r.Text, (f, a) => f.Lorem.Sentence());
            List<E.Book> books = new List<E.Book>();
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                product = ProductFakeData.Create();
                product.Reviews = reviewFaker.GenerateLazy(Random.Shared.Next(0, 5)).ToList();
                books.Add(BookFakeData.Create(product: product));
            }
            await _TestDbContext.Add<E.Book, Guid>(books);

            //Act
            PaginatedDtos<BookSummaryDto> paginatedBookSummaries = await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(new GetBookSummariesQuery
            {
                AverageScore = averageScoreFilter
            });

            //Assert
            int expectedBooksCount = books.Count(a => a.Product.ReviewsAcceptedAverageScore >= averageScoreFilter
                && a.Product.ReviewsAcceptedAverageScore < averageScoreFilter + 1);
            Assert.Equal(expectedBooksCount, paginatedBookSummaries.Dtos.Count);
            _testOutputHelper.WriteLine($"{paginatedBookSummaries.Dtos.Count} books with average score {averageScoreFilter}");
        }



        [Fact]
        public async Task FromCache_ShouldReturn()
        {
            //Arrange
            List<E.Book> books = BookFakeData.CreateBetween(3, 5);
            await _TestDbContext.Add<E.Book, Guid>(books);
            var request = new GetBookSummariesQuery();
            await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(request);

            //Act
            PaginatedDtos<BookSummaryDto>? paginatedBookSummaries = await GetFromCache<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(request);

            //Assert
            Assert.Equal(books.Count, paginatedBookSummaries?.Dtos.Count);
        }


        [Fact]
        public async Task FromCache_WithFilter_CategoryId_ShouldReturn_Filtered()
        {
            //Arrange
            int numberOfCategories = 2;
            List<E.Category> categories = CategoryFakeData.CreateBetween(numberOfCategories, numberOfCategories);
            List<E.Book> books = new List<E.Book>();
            int categoryIndex = 0;
            E.Product product;
            for (int i = 1; i <= 5; i++)
            {
                categoryIndex = Random.Shared.Next(-1, numberOfCategories);
                product = ProductFakeData.Create(category: categoryIndex >= 0 ? categories[categoryIndex] : null);
                books.Add(BookFakeData.Create(product: product));
            }
            await _TestDbContext.Add<E.Book, Guid>(books);
            Guid filterCategoryId = categories.First().Id;
            var request = new GetBookSummariesQuery
            {
                CategoryId = filterCategoryId
            };
            await SendRequest<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(request);

            //Act
            PaginatedDtos<BookSummaryDto>? paginatedBookSummaries = await GetFromCache<GetBookSummariesQuery, PaginatedDtos<BookSummaryDto>>(request);

            //Assert
            List<E.Book> filteredCategories = books.Where(a => a.Product.CategoryId == filterCategoryId).ToList();
            Assert.Equal(filteredCategories.Count, paginatedBookSummaries?.Dtos.Count);
        }






    }
}
