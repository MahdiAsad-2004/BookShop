using BookShop.Application.Features.Discount.Commands.Update;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Exceptions;
using BookShop.IntegrationTest.Application.Common;
using Xunit.Abstractions;

namespace BookShop.IntegrationTest.Application.Discount.Commands
{
    public class UpdateDiscountCommandTest : TestBase
    {
        E.Discount _savedDiscount = new E.Discount()
        {
            Id = Guid.NewGuid(),
            Name = "discount-test",
            CreateBy = string.Empty,
            CreateDate = DateTime.UtcNow,
            LastModifiedBy = string.Empty,
            LastModifiedDate = DateTime.UtcNow,
            DiscountPercent = 50,
            DiscountPrice = null,
            Priority = 1,
            UsedCount = 0,
        };   
        UpdateDiscountCommand updateDiscountCommand = new UpdateDiscountCommand
        {
            Name = "discountUpdate-test",
            DiscountPercent = 80,
            DiscountPrice = null,
            EndDate = DateTime.UtcNow.AddDays(10),
            MaximumUseCount = Random.Shared.Next(10 , 20),
            Priority = Random.Shared.Next(1 , 10),
            StartDate = DateTime.UtcNow.AddDays(9),
        };
        private async Task addDiscount()
        {
            await _TestDbContext.Add<E.Discount, Guid>(_savedDiscount);
        }
        public UpdateDiscountCommandTest(ApplicationCollectionFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            updateDiscountCommand.Id = _savedDiscount.Id;
            addDiscount().GetAwaiter().GetResult();
            SetCurrentUser();
        }







        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            await _TestDbContext.SetPermissionForUser(PermissionConstants.UpdateDiscount);

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            E.Discount? entity = await _TestDbContext.Get<E.Discount,Guid>(updateDiscountCommand.Id);
            Assert.NotNull(entity);
            Assert.Equal(updateDiscountCommand.DiscountPercent, entity.DiscountPercent);
            Assert.Equal(updateDiscountCommand.DiscountPrice, entity.DiscountPrice);
            Assert.Equal(updateDiscountCommand.EndDate, entity.EndDate);
            Assert.Equal(updateDiscountCommand.MaximumUseCount, entity.MaximumUseCount);
            Assert.Equal(updateDiscountCommand.Name, entity.Name);
            Assert.Equal(updateDiscountCommand.Priority, entity.Priority);
            Assert.Equal(updateDiscountCommand.StartDate, entity.StartDate);
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
            await _TestDbContext.Add<E.Discount, Guid>(discount);
            updateDiscountCommand.Name = discount.Name;

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnauthorizeException()
        {
            //Arrange


            //Act
            var task = SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

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
            updateDiscountCommand.DiscountPercent = updateDiscountCommand.DiscountPrice = null;

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.DiscountPercent));
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.DiscountPrice));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_PercentAndPriceBoth_Are_NotNull_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.DiscountPercent = updateDiscountCommand.DiscountPrice = 50;

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.DiscountPercent));
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.DiscountPrice));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Percent_LessThanOrEqual_0_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.DiscountPercent = 0;
            updateDiscountCommand.DiscountPrice = null;

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.DiscountPercent));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Percent_GreaterThan_100_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.DiscountPercent = 100;
            updateDiscountCommand.DiscountPrice = null;

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.DiscountPercent));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Price_LessThanOrEqual_0_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.DiscountPrice = 0;
            updateDiscountCommand.DiscountPercent = null;

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.DiscountPrice));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_EndDate_LessThan_Now_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.EndDate = DateTime.UtcNow.AddDays(-1);

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.EndDate));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_StartDate_LessThan_Now_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.StartDate = DateTime.UtcNow.AddDays(-1);

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.StartDate));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_EndDate_LessThan_StartDate_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.EndDate = DateTime.UtcNow.AddDays(+3);
            updateDiscountCommand.StartDate = DateTime.UtcNow.AddDays(+5);

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.EndDate));
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.StartDate));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_MaximumUseCount_LessThan_0_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.MaximumUseCount = 0;

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.MaximumUseCount));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Priority_LessThanOrEqual_0_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.Priority = 0;

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.Priority));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Name_IsNull_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.Name = null;

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Name_Length_LessThan_2_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.Name = "5";

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Name_Length_GreaterThan_30_ShouldReturn_ValidateionError()
        {
            //Arrange
            updateDiscountCommand.Name = "".PadLeft(31, 'a');

            //Act
            var result = await SendRequest<UpdateDiscountCommand, Result<Empty>>(updateDiscountCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateDiscountCommand.Name));
            _OutPutValidationErrors(result);
        }








    }
}
