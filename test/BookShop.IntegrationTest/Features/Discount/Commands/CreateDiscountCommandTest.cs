using BookShop.Application.Features.Discount.Commands.Create;
using BookShop.Application.Features.Translator.Commands.Create;

namespace BookShop.IntegrationTest.Features.Discount.Commands
{
    public class CreateDiscountCommandTest : TestFeatureBase
    {
        private Result<Empty> result = new Result<Empty>();
        CreateDiscountCommand createDiscountCommand = new CreateDiscountCommand
        {
            Name = "discount-test",
            DiscountPercent = 50,
            DiscountPrice = null,
            EndDate = null,
            MaximumUseCount = null,
            Priority = 1,
            StartDate = null,
        };
        public CreateDiscountCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
             
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<CreateDiscountCommand, Result<Empty>>(createDiscountCommand);
        }





        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.Discount.Add);

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task Duplicate_Name_ShouldReturn_ValidationError()
        {
            //Arrnage 
            E.Discount discount = new E.Discount
            {
                Id = Guid.NewGuid(),
                CreateBy = string.Empty,
                CreateDate = DateTime.UtcNow,
                Name = $"discount-{Random.Shared.Next(1, 100)}",
            };
            await _TestRepository.Add<E.Discount, Guid>(discount);
            createDiscountCommand.Name = discount.Name;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnauthorizeException()
        {
            //Arrange


            //Act
            var task = _TestRequestHandler.SendRequest<CreateDiscountCommand, Result<Empty>>(createDiscountCommand);

            //Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () =>
            {
                await task;
            });
        }


        [Fact]
        public async Task When_PercentAndPriceBoth_Are_Null_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.DiscountPercent = createDiscountCommand.DiscountPrice = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.DiscountPercent));
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.DiscountPrice));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_PercentAndPriceBoth_Are_NotNull_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.DiscountPercent = createDiscountCommand.DiscountPrice = 50;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.DiscountPercent));
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.DiscountPrice));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Percent_LessThanOrEqual_0_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.DiscountPercent = 0;
            createDiscountCommand.DiscountPrice = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.DiscountPercent));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Percent_GreaterThan_100_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.DiscountPercent = 100;
            createDiscountCommand.DiscountPrice = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.DiscountPercent));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Price_LessThanOrEqual_0_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.DiscountPrice = 0;
            createDiscountCommand.DiscountPercent = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.DiscountPrice));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_EndDate_LessThan_Now_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.EndDate = DateTime.UtcNow.AddDays(-1);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.EndDate));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_StartDate_LessThan_Now_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.StartDate = DateTime.UtcNow.AddDays(-1);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.StartDate));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_EndDate_LessThan_StartDate_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.EndDate = DateTime.UtcNow.AddDays(+3);
            createDiscountCommand.StartDate = DateTime.UtcNow.AddDays(+5);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.EndDate));
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.StartDate));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_MaximumUseCount_LessThan_0_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.MaximumUseCount = 0;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.MaximumUseCount));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Priority_LessThanOrEqual_0_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.Priority = 0;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.Priority));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Name_IsNull_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.Name = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Name_Length_LessThan_2_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.Name = "5";

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Name_Length_GreaterThan_30_ShouldReturn_ValidateionError()
        {
            //Arrange
            createDiscountCommand.Name = "".PadLeft(31, 'a');

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createDiscountCommand.Name));
            _OutPutValidationErrors(result);
        }











    }
}
