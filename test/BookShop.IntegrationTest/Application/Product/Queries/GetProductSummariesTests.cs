using Bogus;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Application.Features.Product.Queries.GetSummaries;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Domain.QueryOptions;
using BookShop.Infrstructure.Persistance.DbFunctions;
using BookShop.IntegrationTest.Application.Common;
using BookShop.IntegrationTest.Application.Discount.FakeData;
using BookShop.IntegrationTest.Application.Product.FakeData;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using E = BookShop.Domain.Entities;

namespace BookShop.IntegrationTest.Application.Product.Queries
{
    public class GetProductSummariesTests : TestBase
    {
        public GetProductSummariesTests(ApplicationCollectionFixture applicationCollectionFixture) : base(applicationCollectionFixture)
        {
        }




        [Fact]
        public async Task ShouldReturn()
        {
            //Arrange
            List<E.Product> products = new();
            for (int i = 0; i < 5; i++)
            {
                products.Add(ProductFaker.Create());
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery());


            //Assert
            Assert.Equal(products.Count, productSummaryDtos.Count);
        }


        [Fact]
        public async Task WithFilter_StartPrice_ShouldReturn_Filterd()
        {
            //Arrange
            List<E.Product> products = new();
            int price = 100_000;
            int startPrice = 300_000;
            for (int i = 0; i < 5; i++)
            {
                products.Add(ProductFaker.Create(price: price));
                price += 100_000;
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                StartPrice = startPrice,
            });

            //Asset
            int expectedProductCount = products.Count(a => a.Price >= startPrice);
            Assert.Equal(expectedProductCount, productSummaryDtos.Count);
        }



        [Fact]
        public async Task WithFilter_EndPrice_ShouldReturn_Filterd()
        {
            //Arrange
            List<E.Product> products = new();
            int price = 100_000;
            int endPrice = 400_000;
            for (int i = 0; i < 5; i++)
            {
                products.Add(ProductFaker.Create(price: price));
                price += 100_000;
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                EndPrice = endPrice
            });

            //Asset
            int expectedProductCount = products.Count(a => a.Price <= endPrice);
            Assert.Equal(expectedProductCount, productSummaryDtos.Count);
        }


        [Fact]
        public async Task WithFilter_ProductType_ShouldReturn_Filterd()
        {
            //Arrange
            ProductType productTypeFilter = ProductType.Book;
            List<E.Product> products = new();
            ProductType productType;
            for (int i = 0; i < 5; i++)
            {
                productType = Random.Shared.Next(0, 2) == 0 ? ProductType.Book : ProductType.EBook;
                products.Add(ProductFaker.Create(productType: productType));

            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                ProductType = productTypeFilter,
            });

            //Asset
            int expectedProductsTypeCount = products.Count(a => a.ProductType == productTypeFilter);
            Assert.Equal(expectedProductsTypeCount, productSummaryDtos.Count);
        }


        [Fact]
        public async Task WithFilter_Available_ShouldReturn_Filterd()
        {
            //Arrange
            List<E.Product> products = new();
            for (int i = 0; i < 5; i++)
            {
                bool availableChance = Random.Shared.Next(0, 3) == 0;
                products.Add(ProductFaker.Create(available: availableChance));
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                Available = true
            });

            //Asset
            int expectedProductsCount = products.Count(a => a.NumberOfInventory > 0);
            Assert.Equal(expectedProductsCount, productSummaryDtos.Count);
        }


        [Fact]
        public async Task WithFilter_NotAvailable_ShouldReturn_Filterd()
        {
            //Arrange
            List<E.Product> products = new();
            for (int i = 0; i < 5; i++)
            {
                bool availableChance = Random.Shared.Next(0, 3) == 0;
                products.Add(ProductFaker.Create(available: availableChance));
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                Available = false
            });

            //Asset
            int expectedProductsCount = products.Count(a => a.NumberOfInventory <= 0);
            Assert.Equal(expectedProductsCount, productSummaryDtos.Count);
        }



        [Fact]
        public async Task WithFilter_StartPrice_And_EndPrice_ShouldReturn_Filterd()
        {
            //Arrange
            int startPriceFilter = 200_000;
            int endPriceFilter = 300_000;
            int price = 100_000;
            List<E.Product> products = new();
            for (int i = 0; i < 5; i++)
            {
                products.Add(ProductFaker.Create(price: price));
                price += 100_000;
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                StartPrice = startPriceFilter,
                EndPrice = endPriceFilter,
            });

            //Asset
            int expectedProductsCount = products.Count(a => a.FinalPrice() >= startPriceFilter && a.FinalPrice() <= endPriceFilter);
            Assert.Equal(expectedProductsCount, productSummaryDtos.Count);
        }


        [Fact]
        public async Task WithDiscount_ShouldReturn_Dscounted()
        {
            //Arrange
            int numberOfProductDiscounts = 5;
            List<Product_Discount> product_Discounts = new List<Product_Discount>();
            List<E.Product> products = new List<E.Product>();
            for (int i = 0; i < numberOfProductDiscounts; i++)
            {
                product_Discounts.Add(DiscountFaker.CreateProduct_Discount(DiscountFaker.Create()));
            }
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                product = ProductFaker.Create();
                product.Product_Discounts = new List<Product_Discount>();
                for (int j = 0; j < Random.Shared.Next(2); j++)
                {
                    product.Product_Discounts.Add(product_Discounts[Random.Shared.Next(0, numberOfProductDiscounts)]);
                }
                products.Add(product);
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {

            });

            //Assert
            foreach (var productSummaryDto in productSummaryDtos)
            {
                var p = products.First(a => a.Id.ToString() == productSummaryDto.Id);
                Assert.Equal(p.DiscountedPrice, productSummaryDto.DiscountedPrice);
            }
        }


        [Fact]
        public async Task WithDiscount_WithSomeNotValidDiscount_ShouldReturn_Discounted()
        {
            //Arrange
            int numberOfProductDiscounts = 5;
            Product_Discount product_Discount_NoPercentNoPrice = DiscountFaker.CreateProduct_Discount(DiscountFaker.Create());
            product_Discount_NoPercentNoPrice.Discount.DiscountPercent = product_Discount_NoPercentNoPrice.Discount.DiscountPrice = null;
            Product_Discount product_Discount_EarlyStartDate = DiscountFaker.CreateProduct_Discount(DiscountFaker.Create(startDate: DateTime.UtcNow.AddDays(2)));
            Product_Discount product_Discount_PastEndDate = DiscountFaker.CreateProduct_Discount(DiscountFaker.Create(endDate: DateTime.UtcNow.AddDays(-2)));
            Product_Discount product_Discount_OverPluseMaxUseCount = DiscountFaker.CreateProduct_Discount(DiscountFaker.Create(usedCount: 10, maxUseCount: 10));
            List<Product_Discount> product_Discounts = [product_Discount_EarlyStartDate , product_Discount_NoPercentNoPrice,
            product_Discount_OverPluseMaxUseCount , product_Discount_PastEndDate];
            List<E.Product> products = new List<E.Product>();
            for (int i = 0; i < numberOfProductDiscounts; i++)
            {
                product_Discounts.Add(DiscountFaker.CreateProduct_Discount(DiscountFaker.Create()));
            }
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                product = ProductFaker.Create();
                product.Product_Discounts = new List<Product_Discount>();
                for (int j = 0; j < Random.Shared.Next(2); j++)
                {
                    product.Product_Discounts.Add(product_Discounts[Random.Shared.Next(0, product_Discounts.Count)]);
                }
                products.Add(product);
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {

            });

            //Assert
            foreach (var productSummaryDto in productSummaryDtos)
            {
                var p = products.First(a => a.Id.ToString() == productSummaryDto.Id);
                Assert.Equal(p.DiscountedPrice, productSummaryDto.DiscountedPrice);
            }
        }


        [Fact]
        public async Task WitDiscount_WithEndPriceFilter_ShouldReturn_Filterd()
        {
            //Arrange
            int endPriceFIlter = 600_000;
            E.Product product1 = ProductFaker.Create(price: 1_000_000);
            E.Product product2 = ProductFaker.Create(price: 400_000);
            E.Product product3 = ProductFaker.Create(price: 800_000);
            product1.Product_Discounts = [DiscountFaker.CreateProduct_Discount(DiscountFaker.Create(discountPercent: 50))];
            List<Product_Discount> product_Discounts = new List<Product_Discount>();
            List<E.Product> products = [product1, product2, product3];
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                EndPrice = endPriceFIlter,
            });

            //Asset
            int expectedProductCount = products.Count(a => a.FinalPrice() <= endPriceFIlter);
            Assert.Equal(expectedProductCount, productSummaryDtos.Count);
        }


        [Fact]
        public async Task WithReviews_ReviewsAcceptedAverageScore_MustBe_Correct()
        {
            //Arange
            var reviewFaker = new Faker<Review>();
            reviewFaker.RuleFor(r => r.Score, (f, s) => f.Random.Byte(1, 5));
            reviewFaker.RuleFor(r => r.IsAccepted, (f, a) => f.Random.Bool(0.7f));
            reviewFaker.RuleFor(r => r.Id, (f, a) => Guid.NewGuid());
            reviewFaker.RuleFor(r => r.Text, (f, a) => f.Lorem.Sentence());
            List<E.Product> products = new List<E.Product>();
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                product = ProductFaker.Create();
                product.Reviews = reviewFaker.GenerateLazy(Random.Shared.Next(0, 5)).ToList();
                products.Add(product);
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
            });

            //Assert
            foreach (var productSummaryDto in productSummaryDtos)
            {
                var p = products.First(a => a.Id.ToString() == productSummaryDto.Id);
                Assert.Equal(p.ReviewsAcceptedAverageScore, productSummaryDto.ReviewsAcceptedAverageScore);
            }

        }


        [Fact]
        public async Task WithFilter_AcceptedAverageScore_ShouldReturn_Filtered()
        {
            //Arange
            byte averageScoreFilter = 3;
            var reviewFaker = new Faker<Review>();
            reviewFaker.RuleFor(r => r.Score, (f, s) => f.Random.Byte(1, 5));
            reviewFaker.RuleFor(r => r.IsAccepted, (f, a) => f.Random.Bool(0.7f));
            reviewFaker.RuleFor(r => r.Id, (f, a) => Guid.NewGuid());
            reviewFaker.RuleFor(r => r.Text, (f, a) => f.Lorem.Sentence());
            List<E.Product> products = new List<E.Product>();
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                product = ProductFaker.Create();
                product.Reviews = reviewFaker.GenerateLazy(Random.Shared.Next(0, 5)).ToList();
                products.Add(product);
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                AverageScore = averageScoreFilter
            });

            //Assert
            int expectedProductsCount = products.Count(a => a.ReviewsAcceptedAverageScore >= averageScoreFilter
                && a.ReviewsAcceptedAverageScore < averageScoreFilter + 1);
            Assert.Equal(expectedProductsCount, productSummaryDtos.Count);
        }


        [Fact]
        public async Task WithSort_HighestPrice_ShouldReturn_Ordered()
        {
            //Arrange
            List<E.Product> products = new List<E.Product>();
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                product = ProductFaker.Create(product_Discounts: new List<Product_Discount>());
                for (int j = 0; j < Random.Shared.Next(0, 3); j++)
                {
                    product.Product_Discounts.Add(DiscountFaker.CreateProduct_Discount(DiscountFaker.Create()));
                }
                products.Add(product);
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                SortingOrder = ProductSortingOrder.HighestPrice
            });

            //Assert
            var orderedProducts = products.OrderByDescending(a => a.FinalPrice()).ToArray();
            for (int i = 0; i < orderedProducts.Count(); i++)
            {
                Assert.Equal(orderedProducts[i].Id.ToString(), productSummaryDtos[i].Id);
            }
        }


        [Fact]
        public async Task WithSort_LowestPrice_ShouldReturn_Ordered()
        {
            //Arrange
            List<E.Product> products = new List<E.Product>();
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                product = ProductFaker.Create(product_Discounts: new List<Product_Discount>());
                for (int j = 0; j < Random.Shared.Next(0, 3); j++)
                {
                    product.Product_Discounts.Add(DiscountFaker.CreateProduct_Discount(DiscountFaker.Create()));
                }
                products.Add(product);
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                SortingOrder = ProductSortingOrder.LowestPrice
            });

            //Assert
            var orderedProducts = products.OrderBy(a => a.FinalPrice()).ToArray();
            for (int i = 0; i < orderedProducts.Count(); i++)
            {
                Assert.Equal(orderedProducts[i].Id.ToString(), productSummaryDtos[i].Id);
            }
        }


        [Fact]
        public async Task WithSort_AlphabetDesc_ShouldReturn_Ordered()
        {
            //Arrange
            List<E.Product> products = new List<E.Product>();
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                Bogus.DataSets.Commerce commerce = new Bogus.DataSets.Commerce();
                product = ProductFaker.Create(title: commerce.ProductName());
                products.Add(product);
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                SortingOrder = ProductSortingOrder.AlphabetDesc
            });

            //Assert
            var orderedProducts = products.OrderByDescending(a => a.Title).ToArray();
            for (int i = 0; i < orderedProducts.Count(); i++)
            {
                Assert.Equal(orderedProducts[i].Id.ToString(), productSummaryDtos[i].Id);
            }
        }


        [Fact]
        public async Task WithSort_AlphabetAsce_ShouldReturn_Ordered()
        {
            //Arrange
            List<E.Product> products = new List<E.Product>();
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                Bogus.DataSets.Commerce commerce = new Bogus.DataSets.Commerce();
                product = ProductFaker.Create(title: commerce.ProductName());
                products.Add(product);
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                SortingOrder = ProductSortingOrder.AlphabetAsce
            });

            //Assert
            var orderedProducts = products.OrderBy(a => a.Title).ToArray();
            for (int i = 0; i < orderedProducts.Count(); i++)
            {
                Assert.Equal(orderedProducts[i].Id.ToString(), productSummaryDtos[i].Id);
            }
        }


        [Fact]
        public async Task WithSort_HighestDiscountedPrice_ShouldReturn_Ordered()
        {
            //Arrange
            List<E.Product> products = new List<E.Product>();
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                product = ProductFaker.Create(product_Discounts: new List<Product_Discount>());
                for (int j = 0; j < Random.Shared.Next(1, 3); j++)
                {
                    product.Product_Discounts.Add(DiscountFaker.CreateProduct_Discount(DiscountFaker.Create()));
                }
                products.Add(product);
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                SortingOrder = ProductSortingOrder.HighestDiscount
            });

            //Assert
            var orderedProducts = products.OrderByDescending(a => a.DiscountedPrice).ToArray();
            for (int i = 0; i < orderedProducts.Count(); i++)
            {
                Assert.Equal(orderedProducts[i].Id.ToString(), productSummaryDtos[i].Id);
            }
        }


        [Fact]
        public async Task WithSort_LowestDiscountedPrice_ShouldReturn_Ordered()
        {
            //Arrange
            List<E.Product> products = new List<E.Product>();
            E.Product product;
            for (int i = 0; i < 5; i++)
            {
                product = ProductFaker.Create(product_Discounts: new List<Product_Discount>());
                for (int j = 0; j < Random.Shared.Next(1, 3); j++)
                {
                    product.Product_Discounts.Add(DiscountFaker.CreateProduct_Discount(DiscountFaker.Create()));
                }
                products.Add(product);
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                SortingOrder = ProductSortingOrder.LowestDiscount
            });

            //Assert
            var orderedProducts = products.OrderBy(a => a.DiscountedPrice).ToArray();
            for (int i = 0; i < orderedProducts.Count(); i++)
            {
                Assert.Equal(orderedProducts[i].Id.ToString(), productSummaryDtos[i].Id);
            }
        }


        [Fact]
        public async Task WithSort_HighestSellCount_ShouldReturn_Ordered()
        {
            //Arrange
            List<E.Product> products = new List<E.Product>();
            for (int i = 0; i < 5; i++)
            {
                products.Add(ProductFaker.Create());
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                SortingOrder = ProductSortingOrder.HighestSellCount
            });

            //Assert
            var orderedProducts = products.OrderByDescending(a => a.SellCount).ToArray();
            for (int i = 0; i < orderedProducts.Count(); i++)
            {
                Assert.Equal(orderedProducts[i].Id.ToString(), productSummaryDtos[i].Id);
            }
        }


        [Fact]
        public async Task WithSort_LowestSellCount_ShouldReturn_Ordered()
        {
            //Arrange
            List<E.Product> products = new List<E.Product>();
            for (int i = 0; i < 5; i++)
            {
                products.Add(ProductFaker.Create());
            }
            await _TestDbContext.Add<E.Product, Guid>(products);

            //Act
            List<ProductSummaryDto> productSummaryDtos = await SendRequest<GetProductSummariesQuery, List<ProductSummaryDto>>(new GetProductSummariesQuery
            {
                SortingOrder = ProductSortingOrder.LowestSellCount
            });

            //Assert
            var orderedProducts = products.OrderBy(a => a.SellCount).ToArray();
            for (int i = 0; i < orderedProducts.Count(); i++)
            {
                Assert.Equal(orderedProducts[i].Id.ToString(), productSummaryDtos[i].Id);
            }
        }










    }
}
