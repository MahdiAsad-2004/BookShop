using BookShop.Application.Features.Favorite.Commands.Remove;
using BookShop.IntegrationTest.Features.Product.FakerData;

namespace BookShop.IntegrationTest.Features.Favorite.Commands
{
    public class RemoveFavoriteCommandTest : TestFeatureBase
    {
        private List<E.Favorite> all_Favorites = new();
        private Result<Empty> result = new Result<Empty>();
        private readonly E.Cart cart = new E.Cart
        {
            Id = Guid.NewGuid(),
            UserId = TestCurrentUser.CurrentUserId,
        };
        private E.Product product = ProductFakeData.Create(numberOfInventory: 10);
        private RemoveFavoriteCommand command = new();
        public RemoveFavoriteCommandTest(WebAppFactoryFixture webAppFactoryFixture, ITestOutputHelper testOutputHelper)
            : base(webAppFactoryFixture, testOutputHelper)
        {
            _TestRepository.Add<E.Product, Guid>(product).GetAwaiter().GetResult();
            _TestRepository.Add<E.Cart, Guid>(cart).GetAwaiter().GetResult();
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<RemoveFavoriteCommand, Result<Empty>>(command);
        }
        private async Task getAllFavorites()
        {
            all_Favorites = await _TestRepository.GetAll<E.Favorite, Guid>(a => true);
        }


        [Fact]

        public async Task When_Favorite_Exist_Should_Delete_And_Success_Result()
        {
            //Arrange
            E.Favorite favorite = new E.Favorite
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                UserId = TestCurrentUser.CurrentUserId,
            };
            await _TestRepository.Add<E.Favorite, Guid>(favorite);
            command.FavoriteId = favorite.Id;

            //Act
            await requestAndGetResult();

            //Assert
            Assert.True(result.IsSuccess);
            await getAllFavorites();
            Assert.Equal(0, all_Favorites.Count(a => a.IsDeleted == false));
            _TestOutputHelper.WriteLine($"Result.Message: {result.Message}");
        }

        
        [Fact]
        public async Task When_Favorite_Not_Exist_Should_Return_ValidationError()
        {
            //Arrange
            command.FavoriteId = Guid.NewGuid();   

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(command.FavoriteId));
            _OutPutValidationErrors(result);
        }























    }
}
