
using BookShop.Application.Features.Product.Queries;
using BookShop.Infrastructure.Setting;
using BookShop.IntegrationTest.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BookShop.IntegrationTest
{
    public class Test : TestBase //IClassFixture<TestWebApplicationFactory>
    {
        public Test(WebAppFixture webAppFixture) : base(webAppFixture)
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


        [Fact]
        public async void GetProductQuery_Should_Return_TestProduct()
        { 
            using var scope = _webAppFixture._serviceScopeFactory.CreateScope();
            IConfiguration configuration = scope.ServiceProvider.GetService<IConfiguration>();

            var connectionStringg = configuration.GetConnectionString("DefaultConnection");

            IMediator _mediator = scope.ServiceProvider.GetService<IMediator>();
            var product = await _mediator.Send(new GetProductQuery());

            Assert.NotNull(product);
            Assert.Equal("testProduct", product.Title);

        }










    }
}
