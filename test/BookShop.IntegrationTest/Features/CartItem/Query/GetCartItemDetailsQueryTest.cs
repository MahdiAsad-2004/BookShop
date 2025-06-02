using BookShop.Application.Features.CartItem.Dtos;
using BookShop.Application.Features.CartItem.Query.GetDetails;
using BookShop.Domain.Entities;
using BookShop.IntegrationTest.Features.CartItem.FakeData;
using BookShop.IntegrationTest.Features.Product.FakerData;

namespace BookShop.IntegrationTest.Features.CartItem.Query
{
    public class GetCartItemDetailsQueryTest : TestFeatureBase
    {
        private readonly List<E.Product> products = ProductFakeData.CreateBetween(5, 6);
        private readonly Cart cart = new Cart
        {
            Id = Guid.NewGuid(),
            UserId = TestCurrentUser.CurrentUserId,
        };
        private List<CartItemDetailDto> response = new List<CartItemDetailDto>();
        private GetCartItemDetailsQuery request = new GetCartItemDetailsQuery()
        {
            UserId = TestCurrentUser.CurrentUserId,
        };
        public GetCartItemDetailsQueryTest(WebAppFactoryFixture webAppFactoryFixture, ITestOutputHelper testOutputHelper) 
            : base(webAppFactoryFixture, testOutputHelper)
        {
            _TestRepository.Add<E.Product,Guid> (products).GetAwaiter().GetResult();
            _TestRepository.Add<Cart,Guid> (cart).GetAwaiter().GetResult();
        }

        private async Task sendRequestAnGetResponse()
        {
            response = await _TestRequestHandler.SendRequest<GetCartItemDetailsQuery, List<CartItemDetailDto>>(request);
        }



        [Fact]
        public async Task When_User_Cart_Is_Empty_List_Must_Be_Empty()
        {
            //Arrange


            //Act
            await sendRequestAnGetResponse();

            //Assert
            Assert.Empty(response);
        }


        [Fact]
        public async Task When_User_Cart_Is_Not_Empty_List_Must_Be_Correct()
        {
            //Arrange
            List<E.CartItem> cartItems = CartItemFakeData.CreateBetween(1, 3, products.Select(a => a.Id).ToList(), [cart.Id]);
            await _TestRepository.Add<E.CartItem, Guid>(cartItems);

            //Act
            await sendRequestAnGetResponse();

            //Assert
            Assert.NotEmpty(response);
            Assert.Equal(cartItems.Count, response.Count);
        }




        [Fact]
        public async Task When_Request_Is_Cached_Must_Return_From_Cache()
        {
            //Arrange
            List<E.CartItem> cartItems = CartItemFakeData.CreateBetween(1, 3, products.Select(a => a.Id).ToList(), [cart.Id]);
            await _TestRepository.Add<E.CartItem, Guid>(cartItems);
            await sendRequestAnGetResponse();

            //Act
            List<CartItemDetailDto>? cachedResponse = await _TestCache.GetFromCache<GetCartItemDetailsQuery,List<CartItemDetailDto>>(request);

            //Assert
            Assert.NotNull(cachedResponse);
            Assert.NotEmpty(cachedResponse);
            Assert.Equal(cartItems.Count, cachedResponse.Count);
        }





    }
}
