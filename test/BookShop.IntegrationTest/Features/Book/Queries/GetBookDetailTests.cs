using BookShop.Application.Features.Book.Dtos;
using BookShop.IntegrationTest.Features.Review;
using BookShop.IntegrationTest.Features.Book.FakeData;
using BookShop.IntegrationTest.Features.Author.FakeData;
using BookShop.IntegrationTest.Features.Discount.FakeData;
using BookShop.IntegrationTest.Features.Product.FakerData;
using BookShop.Application.Features.Book.Queries.GetDetail;
using BookShop.IntegrationTest.Features.Translator.FakeData;

namespace BookShop.IntegrationTest.Features.Book.Queries
{
    public class GetBookDetailTests : TestFeatureBase
    {
        public GetBookDetailTests(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        { }


        [Fact]
        public async Task ShouldReturn_WithProduct()
        {
            //Arrange
            E.Book book = BookFakeData.Create(Guid.NewGuid());
            await _TestRepository.Add<E.Book, Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await _TestRequestHandler.SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
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
            E.Product product = ProductFakeData.Create(product_Discounts: new List<E.Product_Discount>());
            for (int i = 0; i < 5; i++)
            {
                product.Product_Discounts.Add(DiscountFakeData.CreateProduct_Discount(DiscountFakeData.Create()));
            }
            E.Book book = BookFakeData.Create(product: product);
            await _TestRepository.Add<E.Book, Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await _TestRequestHandler.SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
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
            E.Product product = ProductFakeData.Create(reviews: ReviewFakeData.CreateBetween(0, 8));
            E.Book book = BookFakeData.Create(product: product);
            await _TestRepository.Add<E.Book, Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await _TestRequestHandler.SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
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
            E.Product product = ProductFakeData.Create(reviews: ReviewFakeData.CreateBetween(0, 6));
            E.Book book = BookFakeData.Create(product: product);
            await _TestRepository.Add<E.Book, Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await _TestRequestHandler.SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
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
            E.Book book = BookFakeData.Create(author_Books: AuthorFakeData.CreateBetween(1, 3, true).ToArray());
            await _TestRepository.Add<E.Book, Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await _TestRequestHandler.SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
            {
                Id = book.Id
            });

            //Assert
            Assert.Equal(book.Id.ToString(), bookDetailDto.Id);
            Assert.Equal(book.Author_Books.Select(a => a.Author).Count(), bookDetailDto.Authors.Count);
        }


        [Fact]
        public async Task WithPublisher_Publisher_MustBe_Correct()
        {
            //Arrange
            E.Book book = BookFakeData.Create(Guid.NewGuid());
            await _TestRepository.Add<E.Book, Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await _TestRequestHandler.SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
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
            await _TestRepository.Add<E.Book, Guid>(book);

            //Act
            BookDetailDto bookDetailDto = await _TestRequestHandler.SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
            {
                Id = book.Id,
            });

            //Assert
            Assert.Equal(book.Id.ToString(), bookDetailDto.Id);
            Assert.Equal(book.Translator.Id, bookDetailDto.Translator.Id);
            Assert.Equal(book.Translator.Name, bookDetailDto.Translator.Name);
        }



        [Fact]
        public async Task ShouldReturn_From_Cache()
        {
            //Arrange
            E.Book book = BookFakeData.Create(Guid.NewGuid());
            await _TestRepository.Add<E.Book, Guid>(book);
            await _TestRequestHandler.SendRequest<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
            {
                Id = book.Id,
            });

            //Act
            BookDetailDto? bookDetailDto = await _TestCache.GetFromCache<GetBookDetailQuery, BookDetailDto>(new GetBookDetailQuery
            {
                Id = book.Id,
            });

            //Assert
            Assert.Equal(bookDetailDto?.Id, book.Id.ToString());
        }






    }
}
