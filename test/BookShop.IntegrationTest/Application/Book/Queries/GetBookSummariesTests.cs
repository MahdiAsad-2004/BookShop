
using BookShop.Application.Common.Dtos;
using BookShop.Application.Features.Book.Dtos;
using BookShop.Application.Features.Book.Queries.GetSummaries;
using BookShop.Domain.QueryOptions;
using BookShop.IntegrationTest.Application.Book.FakeData;
using BookShop.IntegrationTest.Application.Common;

namespace BookShop.IntegrationTest.Application.Book.Queries
{
    public class GetBookSummariesTests : TestBase
    {
        public GetBookSummariesTests(ApplicationCollectionFixture applicationCollectionFixture) : base(applicationCollectionFixture)
        {}


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
            Assert.Equal(books.Count , paginatedBookSummaries.Dtos.Count);
        }


        [Fact]
        public async Task WithSort_PublishYearAsce_ShouldReturn()
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
        public async Task WithSort_PublishYearDesc_ShouldReturn()
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
        public async Task WithFilter_StartPublishYear_ShouldReturn()
        {
            //Arrange
            List<E.Book> books = new List<E.Book>();
            DateTime publishYear = DateTime.Now;
            DateTime startPublishYear = DateTime.Now.AddMonths(Random.Shared.Next(-4 , 0)).AddDays(-10);
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
        public async Task WithFilter_EndPublishYear_ShouldReturn()
        {
            //Arrange
            List<E.Book> books = new List<E.Book>();
            DateTime publishYear = DateTime.Now;
            DateTime endPublishYear = DateTime.Now.AddMonths(Random.Shared.Next(-4 , 0)).AddDays(+10);
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












    }
}
