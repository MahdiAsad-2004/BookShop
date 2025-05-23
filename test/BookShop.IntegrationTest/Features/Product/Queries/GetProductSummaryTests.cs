﻿using BookShop.Application.Features.Product.Dtos;
using BookShop.Application.Features.Product.Queries.GetSummary;
using BookShop.IntegrationTest.Features.Discount.FakeData;
using BookShop.IntegrationTest.Features.Product.FakerData;
using BookShop.IntegrationTest.Features.Review;

namespace BookShop.IntegrationTest.Features.Product.Queries
{
    public class GetProductSummaryTests : TestFeatureBase 
    {
        public GetProductSummaryTests(WebAppFactoryFixture webAppFixture, ITestOutputHelper testOutputHelper)
            : base(webAppFixture, testOutputHelper)
        {
        }



        [Fact]
        public async Task ByTitle_ShouldReturn()
        {
            //Arange
            Guid productId = Guid.NewGuid();
            string productTitle = "product1";
            var product = ProductFakeData.Create(productId, productTitle);
            await _TestRepository.Add<E.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await _TestRequestHandler.SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
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
            var product = ProductFakeData.Create(productId);
            await _TestRepository.Add<E.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await _TestRequestHandler.SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
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
            E.Product_Discount product_Discount = DiscountFakeData.CreateProduct_Discount(DiscountFakeData.Create(discountPercent));
            var product = ProductFakeData.Create(productId, price);
            product.Product_Discounts = [product_Discount];
            await _TestRepository.Add<E.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await _TestRequestHandler.SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
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
            E.Product_Discount product_Discount = DiscountFakeData.CreateProduct_Discount(DiscountFakeData.Create(discountPrice));
            var product = ProductFakeData.Create(productId, price: price, product_Discounts: [product_Discount]);
            product.Product_Discounts = [product_Discount];
            await _TestRepository.Add<E.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await _TestRequestHandler.SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            float expectedDiscountedPrice = (float)price - discountPrice;
            Assert.Equal(productId.ToString(), productSummaryDto.Id);
            Assert.Equal(expectedDiscountedPrice, productSummaryDto.DiscountedPrice);
        }


        [Fact]
        public async Task ById_WithDiscount_WithPriority_ShouldReturn_MostPriorityValidDiscount()
        {
            //Arange
            int price = 1_000_000;
            int discountPrice = 0;
            Guid productId = Guid.NewGuid();
            List<E.Product_Discount> product_Discounts = new();
            for (int i = 0; i < 5; i++)
            {
                discountPrice = Random.Shared.Next(1_000, 900_000);
                product_Discounts.Add(DiscountFakeData.CreateProduct_Discount(DiscountFakeData.Create(discountPrice)));
            }
            var product = ProductFakeData.Create(productId, price: price, product_Discounts: product_Discounts);
            await _TestRepository.Add<E.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await _TestRequestHandler.SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = productId,
            });

            E.Discount mostPriorityDiscount = product_Discounts.Select(a => a.Discount).Where(a => a.IsValid()).OrderBy(a => a.Priority).First();
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
            E.Product_Discount product_Discount1 = DiscountFakeData.CreateProduct_Discount(DiscountFakeData.Create(startDate: discountStartDate));
            E.Product product = ProductFakeData.Create(productId, product_Discounts: [product_Discount1]);
            await _TestRepository.Add<E.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await _TestRequestHandler.SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
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
            E.Product_Discount product_Discount1 = DiscountFakeData.CreateProduct_Discount(DiscountFakeData.Create(endDate: discountEndDate));
            E.Product product = ProductFakeData.Create(productId, product_Discounts: [product_Discount1]);
            await _TestRepository.Add<E.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await _TestRequestHandler.SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
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
            E.Discount discount = DiscountFakeData.Create();
            discount.DiscountPercent = discount.DiscountPrice = null;
            E.Product_Discount product_Discount1 = DiscountFakeData.CreateProduct_Discount(discount);
            E.Product product = ProductFakeData.Create(productId, product_Discounts: [product_Discount1]);
            await _TestRepository.Add<E.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await _TestRequestHandler.SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
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
            E.Product product = ProductFakeData.Create(reviews: ReviewFakeData.CreateBetween(5, 10));
            await _TestRepository.Add<E.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await _TestRequestHandler.SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
            {
                Id = product.Id,
            });

            //Assert
            float expectedReviewsAcceptedAverageScore = (float)product.Reviews.Where(a => a.IsAccepted).Average(a => a.Score);
            Assert.Equal(product.Id.ToString(), productSummaryDto.Id);
            Assert.Equal(expectedReviewsAcceptedAverageScore, productSummaryDto.ReviewsAcceptedAverageScore);
        }


        [Fact]
        public async Task ById_WithReviews_WithAllNotAccepted_AverageReviewScore_MustBe_Zero()
        {
            //Arange
            Guid productId = Guid.NewGuid();
            var reviewFaker = new Faker<E.Review>();
            reviewFaker.RuleFor(r => r.Score, (f, s) => f.Random.Byte(1, 5));
            reviewFaker.RuleFor(r => r.IsAccepted, (f, a) => false);
            reviewFaker.RuleFor(r => r.Id, (f, a) => Guid.NewGuid());
            reviewFaker.RuleFor(r => r.Text, (f, a) => f.Lorem.Sentence());
            reviewFaker.RuleFor(r => r.ProductId, (f, a) => productId);
            List<E.Review> reviews = reviewFaker.GenerateLazy(5).ToList();
            E.Product product = ProductFakeData.Create(productId);
            product.Reviews = reviews;
            await _TestRepository.Add<E.Product, Guid>(product);

            //Act
            ProductSummaryDto productSummaryDto = await _TestRequestHandler.SendRequest<GetProductSummaryQuery, ProductSummaryDto>(new GetProductSummaryQuery
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
