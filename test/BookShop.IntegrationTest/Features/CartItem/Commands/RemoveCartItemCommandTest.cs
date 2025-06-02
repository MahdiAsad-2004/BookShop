using BookShop.Application.Features.CartItem.Commands.Remove;
using BookShop.IntegrationTest.Features.Product.FakerData;

namespace BookShop.IntegrationTest.Features.CartItem.Commands
{
    public class RemoveCartItemCommandTest : TestFeatureBase
    {
        private List<E.CartItem> all_cartItems = new();
        private Result<Empty> result = new Result<Empty>();
        private readonly E.Cart cart = new E.Cart
        {
            Id = Guid.NewGuid(),
            UserId = TestCurrentUser.CurrentUserId,
        };
        private E.Product product = ProductFakeData.Create(numberOfInventory: 10);
        private RemoveCartItemCommand command = new();
        public RemoveCartItemCommandTest(WebAppFactoryFixture webAppFactoryFixture, ITestOutputHelper testOutputHelper)
            : base(webAppFactoryFixture, testOutputHelper)
        {
            _TestRepository.Add<E.Product, Guid>(product).GetAwaiter().GetResult();
            _TestRepository.Add<E.Cart, Guid>(cart).GetAwaiter().GetResult();
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<RemoveCartItemCommand, Result<Empty>>(command);
        }
        private async Task getAllCartItems()
        {
            all_cartItems = await _TestRepository.GetAll<E.CartItem, Guid>(a => true);
        }


        [Fact]

        public async Task When_CartItem_Exist_Should_Delete_And_Success_Result()
        {
            //Arrange
            E.CartItem cartItem = new E.CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = 1,
            };
            await _TestRepository.Add<E.CartItem, Guid>(cartItem);
            command.CartItemId = cartItem.Id;

            //Act
            await requestAndGetResult();

            //Assert
            await getAllCartItems();
            Assert.Equal(0, all_cartItems.Count(a => a.IsDeleted == false));
            Assert.True(result.IsSuccess);
            _TestOutputHelper.WriteLine($"Result.Message: {result.Message}");
        }

        
        [Fact]
        public async Task When_CartItem_Not_Exist_Should_Return_ValidationError()
        {
            //Arrange
            command.CartItemId = Guid.NewGuid();   

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(command.CartItemId));
            _OutPutValidationErrors(result);
        }























    }
}
