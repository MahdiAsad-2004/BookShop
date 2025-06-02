using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Application.Features.CartItem.Commands.Create;
using BookShop.IntegrationTest.Features.Product.FakerData;

namespace BookShop.IntegrationTest.Features.CartItem.Commands
{
    public class CreateCartItemCommandTest : TestFeatureBase
    {
        private E.CartItem? result_cartItem = new();
        private Result<Empty> result = new Result<Empty>();
        private E.Product product = ProductFakeData.Create(numberOfInventory: 10);
        private CreateCartItemCommand command = new CreateCartItemCommand()
        {
            Quantity = 1
        };
        public CreateCartItemCommandTest(WebAppFactoryFixture webAppFactoryFixture, ITestOutputHelper testOutputHelper)
            : base(webAppFactoryFixture, testOutputHelper)
        {
            _TestRepository.Add<E.Product, Guid>(product)
                .GetAwaiter()
                .GetResult();
            command.ProductId = product.Id;
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<CreateCartItemCommand, Result<Empty>>(command);
        }
        private async Task getResultCartItem()
        {
            result_cartItem = null;
            var cartItems = await _TestRepository.GetAll<E.CartItem, Guid>(a => true);
            if (cartItems != null && cartItems.Any())
            {
                result_cartItem = cartItems[0];
            }
        }



        [Fact]
        public async Task When_Cart_NotExist_Should_Create_Cart_And_CartItem()
        {
            //Arrage


            //Act
            await requestAndGetResult();

            //Assert
            await getResultCartItem();
            Assert.NotNull(result_cartItem);
            Assert.Equal(product.Id, result_cartItem.ProductId);
            Assert.Equal(command.Quantity, result_cartItem.Quantity);
        }


        [Fact]
        public async Task When_Cart_Exist_And_CartItem_NotExist_Should_Create_CartItem()
        {
            //Arrage
            E.Cart cart = new E.Cart
            {
                Id = Guid.NewGuid(),
                UserId = TestCurrentUser.CurrentUserId,
            };
            await _TestRepository.Add<E.Cart, Guid>(cart);

            //Act
            await requestAndGetResult();

            //Assert
            await getResultCartItem();
            Assert.NotNull(result_cartItem);
            Assert.Equal(product.Id, result_cartItem.ProductId);
            Assert.Equal(command.Quantity, result_cartItem.Quantity);
            Assert.Equal(cart.Id, result_cartItem.CartId);
        }


        [Fact]
        public async Task When_Cart_Exist_And_CartItem_NotExist_Should_Update_CartItem()
        {
            //Arrange
            E.CartItem cartItem = new E.CartItem
            {
                ProductId = product.Id,
                Quantity = 5,
            };
            E.Cart cart = new E.Cart
            {
                Id = Guid.NewGuid(),
                UserId = TestCurrentUser.CurrentUserId,
                CartItems = new List<E.CartItem>
                {
                    cartItem
                }
            };
            await _TestRepository.Add<E.Cart, Guid>(cart);

            //Act
            await requestAndGetResult();

            //Assert
            await getResultCartItem();
            Assert.NotNull(result_cartItem);
            Assert.Equal(product.Id, result_cartItem.ProductId);
            Assert.Equal(cartItem.Quantity + command.Quantity, result_cartItem.Quantity);
            Assert.Equal(cart.Id, result_cartItem.CartId);
        }


        [Fact]
        public async Task When_CartItem_NotExist_And_Requested_Quantity_GreaterThan_Stock_Should_Create_CartItem_With_MaxStock_Quantity()
        {
            //Arrange
            E.Cart cart = new E.Cart
            {
                Id = Guid.NewGuid(),
                UserId = TestCurrentUser.CurrentUserId,
            };
            await _TestRepository.Add<E.Cart, Guid>(cart);
            command.Quantity = 50;

            //Act
            await requestAndGetResult();

            //Assert
            await getResultCartItem();
            Assert.NotNull(result_cartItem);
            Assert.Equal(product.Id, result_cartItem.ProductId);
            Assert.Equal(product.NumberOfInventory, result_cartItem.Quantity);
            Assert.Equal(cart.Id, result_cartItem.CartId);
        }



        [Fact]
        public async Task When_CartItem_NotExist_And_Requested_Quantity_GreaterThan_Stock_Should_Update_CartItem_With_MaxStock_Quantity()
        {
            //Arrange
            E.CartItem cartItem = new E.CartItem
            {
                ProductId = product.Id,
                Quantity = 5
            };
            E.Cart cart = new E.Cart
            {
                Id = Guid.NewGuid(),
                UserId = TestCurrentUser.CurrentUserId,
                CartItems = [cartItem]
            };
            await _TestRepository.Add<E.Cart, Guid>(cart);
            command.Quantity = 10;

            //Act
            await requestAndGetResult();

            //Assert
            await getResultCartItem();
            Assert.NotNull(result_cartItem);
            Assert.Equal(product.Id, result_cartItem.ProductId);
            Assert.Equal(product.NumberOfInventory, result_cartItem.Quantity);
            Assert.Equal(cart.Id, result_cartItem.CartId);
        }


        [Fact]
        public async Task When_ProductId_Not_Exist_Should_Return_ValidationError()
        {
            //Arrange
            command.ProductId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(command.ProductId));
            _OutPutValidationErrors(result);
        }

        
        [Fact]
        public async Task When_Quantity_LessThan_1_Should_Return_ValidationError()
        {
            //Arrange
            command.Quantity = 0;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(command.Quantity));
            _OutPutValidationErrors(result);
        }





    }
}
