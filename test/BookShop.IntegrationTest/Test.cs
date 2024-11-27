
using BookShop.Application.Features.Product.Queries;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Infrastructure.Setting;
using BookShop.IntegrationTest.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BookShop.IntegrationTest
{
    public class Test : TestBase //IClassFixture<TestWebApplicationFactory>
    {
        public Test(ApplicationCollectionFixture webAppFixture) : base(webAppFixture)
        {
        }

        //private readonly TestWebApplicationFactory _factory;
        ////private readonly IServiceScopeFactory _serviceScopeFactory;
        //public Test(TestWebApplicationFactory factory)
        //{
        //    _factory = factory;
        //    //_factory = new TestWebApplicationFactory();
        //    //_serviceScopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();


        //}


        //[Fact]
        //public async void GetProductQuery_Should_Return_TestProduct()
        //{
        //    var product = new Domain.Entities.Product()
        //    {
        //        CreateBy = string.Empty,
        //        CreateDate = DateTime.UtcNow,
        //        DeleteDate = null,
        //        DeletedBy = null,
        //        DescriptionHtml = string.Empty,
        //        Id = Guid.NewGuid(),
        //        ImageName = "testImage.png",
        //        IsDeleted = false,
        //        LastModifiedBy = string.Empty,
        //        LastModifiedDate = DateTime.UtcNow,
        //        NumberOfInventory = 10,
        //        Price = 1_000_000,
        //        ProductType = ProductType.Book,
        //        SellCount = 5,
        //        Title = "TestProduct0",
        //    };
        //    await _testDbContext.Add<Domain.Entities.Product, Guid>(product);

        //    Assert.True(true);
        //}










    }
}
