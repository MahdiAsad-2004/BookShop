using BookShop.Application.Features.Favorite.Commands.Create;
using BookShop.IntegrationTest.Features.Product.FakerData;

namespace BookShop.IntegrationTest.Features.Favorite.Commands
{
    public class CreateFavoriteCommandTest : TestFeatureBase
    {
        private Result<Empty> result;
        private CreateFavoriteCommand command = new CreateFavoriteCommand();
        private readonly E.Product product = ProductFakeData.Create();
        public CreateFavoriteCommandTest(WebAppFactoryFixture webAppFactoryFixture, ITestOutputHelper testOutputHelper)
            : base(webAppFactoryFixture, testOutputHelper)
        {
            _TestRepository.Add<E.Product , Guid>(product).GetAwaiter().GetResult();
            command.ProductId = product.Id;
            command.UserId = TestCurrentUser.CurrentUserId;
        }
        public async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<CreateFavoriteCommand, Result<Empty>>(command);
        }




        [Fact]
        public async Task When_Request_Is_Correct_Should_Create_And_Return_Success()
        {
            //Arrange

            //Act
            await requestAndGetResult();

            //Assert
            Assert.True(result.IsSuccess);
            List<E.Favorite> userFavorites = await _TestRepository.GetAll<E.Favorite, Guid>(a => a.UserId == command.UserId);
            Assert.NotEmpty(userFavorites);
            Assert.Single(userFavorites);
            _TestOutputHelper.WriteLine($"Result.Message: {result.Message}");
        }
        

        [Fact]
        public async Task When_Product_Exist_In_User_Favorites_Should_Return_Fail()
        {
            //Arrange
            E.Favorite favorite = new E.Favorite
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                UserId = TestCurrentUser.CurrentUserId
            };
            await _TestRepository.Add<E.Favorite, Guid>(favorite);

            //Act
            await requestAndGetResult();

            //Assert
            Assert.False(result.IsSuccess);
            List<E.Favorite> userFavorites = await _TestRepository.GetAll<E.Favorite, Guid>(a => a.UserId == command.UserId);
            Assert.NotEmpty(userFavorites);
            Assert.Single(userFavorites);
            _TestOutputHelper.WriteLine($"Result.Message: {result.Message}");
        }




        [Fact]
        public async Task When_UserId_Not_Exist_Shoud_Return_ValidatioNError()
        {
            //Arrange
            command.UserId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(command.UserId));
            _OutPutValidationErrors(result);
        }
        

        [Fact]
        public async Task When_ProductId_Not_Exist_Shoud_Return_ValidatioNError()
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










        










    }
}
