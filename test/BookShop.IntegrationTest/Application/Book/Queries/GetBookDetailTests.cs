using BookShop.IntegrationTest.Application.Common;
using BookShop.IntegrationTest.Application.Product.FakeData;
using BookShop.IntegrationTest.Application.Book.FakeData;
using BookShop.Application.Features.Book.Dtos;
using BookShop.Application.Features.Book.Queries.GetDetail;
using BookShop.Domain.Entities;
using BookShop.IntegrationTest.Application.Discount.FakeData;
using BookShop.IntegrationTest.Application.Review;
using BookShop.IntegrationTest.Application.Author.FakeData;
using BookShop.IntegrationTest.Application.Translator.FakeData;

namespace BookShop.IntegrationTest.Application.Book.Queries
{
    public class GetBookDetailTests : TestBase
    {
        public GetBookDetailTests(ApplicationCollectionFixture applicationCollectionFixture) : base(applicationCollectionFixture)
        {}


        [Fact]
        public async Task ShouldReturn_WithProduct()
        {
            //Arrange
            E.Book book = BookFakeData.Create(Guid.NewGuid());
            await _TestDbContext.Add<E.Book,Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
            {
                Id = book.Id,
            });

            //Assert
            Assert.Equal(book.Id.ToString(), bookDetailDto.Id);
            Assert.Equal(book.Product.Price, bookDetailDto.Price);
            Assert.Equal(book.Product.Title, bookDetailDto.Title);
        }


        [Fact]
        public async Task WithDiscount_ShouldReturn_Discounted()
        {
            //Arrange
            E.Product product = ProductFakeData.Create(product_Discounts:new List<Product_Discount>());
            for (int i = 0; i < 5; i++)
            {
                product.Product_Discounts.Add(DiscountFakeData.CreateProduct_Discount(DiscountFakeData.Create()));
            }
            E.Book book = BookFakeData.Create(product: product);
            await _TestDbContext.Add<E.Book , Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await SendRequest<GetBookDetailQuery,BookDetailDto>(new GetBookDetailQuery 
            { 
                Id = book.Id,
            });

            //Assert
            Assert.Equal(book.Id.ToString(), bookDetailDto.Id);
            Assert.Equal(book.Product.DiscountedPrice, bookDetailDto.DiscountedPrice);
        }


        [Fact]
        public async Task WithReviews_ReviewsAcceptedCount_MustBe_Correct()
        {
            //Arrange
            E.Product product = ProductFakeData.Create(reviews: ReviewFakeData.CreateBetween(0,8));
            E.Book book = BookFakeData.Create(product: product);
            await _TestDbContext.Add<E.Book,Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
            {
                Id = book.Id
            });

            //Assert
            Assert.Equal(book.Id.ToString(), bookDetailDto.Id);
            Assert.Equal(book.Product.Reviews.Count(a => a.IsAccepted), bookDetailDto.ReviewsAccepted.Count);
        }


        [Fact]
        public async Task WithReviews_ReviewsAcceptedAverageScore_MustBe_Correct()
        {
            //Arrange
            E.Product product = ProductFakeData.Create(reviews: ReviewFakeData.CreateBetween(0,6));
            E.Book book = BookFakeData.Create(product: product);
            await _TestDbContext.Add<E.Book,Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
            {
                Id = book.Id
            });

            //Assert
            Assert.Equal(book.Id.ToString(), bookDetailDto.Id);
            Assert.Equal(book.Product.ReviewsAcceptedAverageScore, bookDetailDto.ReviewsAcceptedAverageScore);
        }


        [Fact]
        public async Task WithAuthors_AuthorsCount_MustBe_Correct()
        {
            //Arrange
            E.Book book = BookFakeData.Create(authors:AuthorFakeData.CreateBetween(0,3));
            await _TestDbContext.Add<E.Book, Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
            {
                Id = book.Id
            });

            //Assert
            Assert.Equal(book.Id.ToString(), bookDetailDto.Id);
            Assert.Equal(book.Authors.Count, bookDetailDto.Authors.Count);
        }
        

        [Fact]
        public async Task WithPublisher_Publisher_MustBe_Correct()
        {
            //Arrange
            E.Book book = BookFakeData.Create(Guid.NewGuid());
            await _TestDbContext.Add<E.Book, Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
            {
                Id = book.Id
            });

            //Assert
            Assert.Equal(book.Id.ToString(), bookDetailDto.Id);
            Assert.Equal(book.Publisher.Id, bookDetailDto.Publisher.Id);
            Assert.Equal(book.Publisher.Title, bookDetailDto.Publisher.Title);
        }


        [Fact]
        public async Task WithTranslator_Translator_MustBe_Correct()
        {
            //Arrange
            E.Book book = BookFakeData.Create(translator: TranslatorFakeData.Create());
            await _TestDbContext.Add<E.Book, Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
            {
                Id = book.Id
            });

            //Assert
            Assert.Equal(book.Id.ToString(), bookDetailDto.Id);
            Assert.Equal(book.Translator.Id, bookDetailDto.Translator.Id);
            Assert.Equal(book.Translator.Name, bookDetailDto.Translator.Name);
        }










    }
}
