
using Bogus;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Application.Features.Product.Queries.GetSummary;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.IntegrationTest.Application.Common;
using BookShop.IntegrationTest.Application.Discount.FakeData;
using BookShop.IntegrationTest.Application.Product.FakeData;

namespace BookShop.IntegrationTest.Application.Product.Queries
{
    public class GetProductSummaryTests : TestBase //IClassFixture<ApplicationClassFixture>
    {
        public GetProductSummaryTests(ApplicationCollectionFixture webAppFixture) : base(webAppFixture)
        {
        }

        //private readonly ApplicationClassFixture _applicationClassFixture;
        //public GetProductSummaryTests(ApplicationClassFixture applicationClassFixture) 
        //{
        //    _applicationClassFixture = applicationClassFixture;
        //}

        //private readonly ApplicationCollectionFixture _applicationCollectionFixture;
        //public GetProductSummaryTests(ApplicationCollectionFixture applicationCollectionFixture)
        //{
        //    _applicationCollectionFixture = applicationCollectionFixture;
        //}



        [Fact]
        public async Task ByTitle_ShouldReturn()
        {
            //Arange
            Guid productId = Guid.NewGuid();
            string productTitle = "product1";
            var product = ProductFaker.Create(productId, productTitle);
            await _TestDbContext.Add<Domain.Entities.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Title = productTitle,
            });

            //Assert
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
            Assert.Equal(productTitle, productSummaryDto.Title);
        }


        [Fact]
        public async Task ById_ShouldReturn()
        {
            //Arange
            Guid productId = Guid.NewGuid();
            var product = ProductFaker.Create(productId);
            await _TestDbContext.Add<Domain.Entities.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            //Assert
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
        }


        [Fact]
        public async Task ById_WithDiscount_WithPercent_ShouldReturn_Discounted()
        {
            //Arange
            int price = 1_000_000;
            byte discountPercent = (byte)Random.Shared.Next(1, 100);
            Guid productId = Guid.NewGuid();
            Product_Discount product_Discount = DiscountFaker.CreateProduct_Discount(DiscountFaker.Create(discountPercent));
            var product = ProductFaker.Create(productId, price);
            product.Product_Discounts = [product_Discount];
            await _TestDbContext.Add<Domain.Entities.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            float expectedDiscountedPrice = price - price * (float)discountPercent / 100f;
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
            Assert.Equal(expectedDiscountedPrice, productSummaryDto.DiscountedPrice);
        }


        [Fact]
        public async Task ById_WithDiscount_WithPrice_ShouldReturn_Discounted()
        {
            //Arange
            int price = 1_000_000;
            int discountPrice = Random.Shared.Next(1_000, 900_000);
            Guid productId = Guid.NewGuid();
            Product_Discount product_Discount = DiscountFaker.CreateProduct_Discount(DiscountFaker.Create(discountPrice));
            var product = ProductFaker.Create(productId, price: price, product_Discounts: [product_Discount]);
            product.Product_Discounts = [product_Discount];
            await _TestDbContext.Add<Domain.Entities.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            float expectedDiscountedPrice = (float)price - discountPrice;
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
            Assert.Equal(expectedDiscountedPrice, productSummaryDto.DiscountedPrice);
        }


        [Fact]
        public async Task ById_WithDiscount_WithPriority_ShouldReturn_MostPriorityDiscount()
        {
            //Arange
            int price = 1_000_000;
            int discountPrice = 0;
            Guid productId = Guid.NewGuid();
            List<Product_Discount> product_Discounts = new();
            for (int i = 0; i < 5; i++)
            {
                discountPrice = Random.Shared.Next(1_000, 900_000);
                product_Discounts.Add(DiscountFaker.CreateProduct_Discount(DiscountFaker.Create(discountPrice)));
            }
            var product = ProductFaker.Create(productId, price: price, product_Discounts: product_Discounts);
            await _TestDbContext.Add<Domain.Entities.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            Domain.Entities.Discount mostPriorityDiscount = product_Discounts.Select(a => a.Discount).OrderBy(a => a.Priority).First();
            float expectedDiscountedPrice = (float)price - mostPriorityDiscount.DiscountPrice.Value;
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
            Assert.Equal(expectedDiscountedPrice, productSummaryDto.DiscountedPrice);
        }


        [Fact]
        public async Task ById_WithDiscount_WithEarlyStartDate_ShouldReturn_WithoutDiscounted()
        {
            //Arange
            DateTime discountStartDate = DateTime.UtcNow.AddHours(2);
            Guid productId = Guid.NewGuid();
            Product_Discount product_Discount1 = DiscountFaker.CreateProduct_Discount(DiscountFaker.Create(startDate: discountStartDate));
            Domain.Entities.Product product = ProductFaker.Create(productId, product_Discounts: [product_Discount1]);
            await _TestDbContext.Add<Domain.Entities.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            float? expectedDiscountedPrice = null;
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
            Assert.Equal(expectedDiscountedPrice, productSummaryDto.DiscountedPrice);
        }



        [Fact]
        public async Task ById_WithDiscount_WithPastEndDateDate_ShouldReturn_WithoutDiscounted()
        {
            //Arange
            DateTime discountEndDate = DateTime.UtcNow.AddDays(-2);
            Guid productId = Guid.NewGuid();
            Product_Discount product_Discount1 = DiscountFaker.CreateProduct_Discount(DiscountFaker.Create(endDate: discountEndDate));
            Domain.Entities.Product product = ProductFaker.Create(productId, product_Discounts: [product_Discount1]);
            await _TestDbContext.Add<Domain.Entities.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            float? expectedDiscountedPrice = null;
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
            Assert.Equal(expectedDiscountedPrice, productSummaryDto.DiscountedPrice);
        }



        [Fact]
        public async Task ById_WithDiscount_WithoutPercentAndPrice_ShouldReturn_WithoutDiscounted()
        {
            //Arange
            Guid productId = Guid.NewGuid();
            Domain.Entities.Discount discount = DiscountFaker.Create();
            discount.DiscountPercent = discount.DiscountPrice = null;
            Product_Discount product_Discount1 = DiscountFaker.CreateProduct_Discount(discount);
            Domain.Entities.Product product = ProductFaker.Create(productId, product_Discounts: [product_Discount1]);
            await _TestDbContext.Add<Domain.Entities.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            float? expectedDiscountedPrice = null;
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
            Assert.Equal(expectedDiscountedPrice, productSummaryDto.DiscountedPrice);
        }


        [Fact]
        public async Task ById_WithReviews_ShouldReturn_AverageReviewScore()
        {
            //Arange
            Guid productId = Guid.NewGuid();
            var reviewFaker = new Faker<Review>();
            reviewFaker.RuleFor(r => r.Score, (f, s) => f.Random.Byte(1, 5));
            reviewFaker.RuleFor(r => r.IsAccepted, (f, a) => f.Random.Bool(0.7f));
            reviewFaker.RuleFor(r => r.Id, (f, a) => Guid.NewGuid());
            reviewFaker.RuleFor(r => r.Text, (f, a) => f.Lorem.Sentence());
            reviewFaker.RuleFor(r => r.ProductId, (f, a) => productId);
            List<Review> reviews = reviewFaker.GenerateLazy(10).ToList();
            Domain.Entities.Product product = ProductFaker.Create(productId);
            product.Reviews = reviews;
            await _TestDbContext.Add<Domain.Entities.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            //Assert
            float expectedReviewsAcceptedAverageScore = (float)reviews.Where(a => a.IsAccepted).Average(a => a.Score);
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
            Assert.Equal(expectedReviewsAcceptedAverageScore, productSummaryDto.ReviewsAcceptedAverageScore);
        }


        [Fact]
        public async Task ById_WithReviews_WithAllNotAccepted_AverageReviewScore_MustBe_Zero()
        {
            //Arange
            Guid productId = Guid.NewGuid();
            var reviewFaker = new Faker<Review>();
            reviewFaker.RuleFor(r => r.Score, (f, s) => f.Random.Byte(1, 5));
            reviewFaker.RuleFor(r => r.IsAccepted, (f, a) => false);
            reviewFaker.RuleFor(r => r.Id, (f, a) => Guid.NewGuid());
            reviewFaker.RuleFor(r => r.Text, (f, a) => f.Lorem.Sentence());
            reviewFaker.RuleFor(r => r.ProductId, (f, a) => productId);
            List<Review> reviews = reviewFaker.GenerateLazy(5).ToList();
            Domain.Entities.Product product = ProductFaker.Create(productId);
            product.Reviews = reviews;
            await _TestDbContext.Add<Domain.Entities.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            //Assert
            float expectedReviewsAcceptedAverageScore = 0f;
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
            Assert.Equal(expectedReviewsAcceptedAverageScore, productSummaryDto.ReviewsAcceptedAverageScore);
        }








    }
}
