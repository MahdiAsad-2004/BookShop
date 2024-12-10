
using BookShop.Application.Common.Dtos;
using BookShop.Application.Features.Book.Dtos;
using BookShop.Application.Features.Book.Queries.GetSummaries;
using BookShop.Domain.Entities;
using BookShop.Domain.QueryOptions;
using BookShop.IntegrationTest.Application.Book.FakeData;
using BookShop.IntegrationTest.Application.Category.FakeData;
using BookShop.IntegrationTest.Application.Common;
using BookShop.IntegrationTest.Application.Product.FakeData;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BookShop.IntegrationTest.Application.Book.Queries
{
    public class GetBookSummariesTests : TestBase
    {
        public GetBookSummariesTests(ApplicationCollectionFixture applicationCollectionFixture) : base(applicationCollectionFixture)
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
            List<E.Category> parentCategories = CategoryFakeData.CreateBetween(2 , 2);
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
            List<Guid> allChildIds = categories.Where(a => a.Parent.Id == filterParentCategoryId).Select(a => a.Id).ToList();
            allChildIds.Add(filterParentCategoryId);
            List<E.Book> filteredCategories = books.Where(a => allChildIds.Contains(a.Product.CategoryId ?? Guid.Empty)).ToList();
            Assert.Equal(filteredCategories.Count, paginatedBookSummaries.Dtos.Count);
        }








    }
}
