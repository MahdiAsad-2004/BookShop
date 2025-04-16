using BookShop.Application.Extensions;
using BookShop.Application.Features.Translator.Commands.Update;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using BookShop.IntegrationTest.Application.Common;
using Xunit.Abstractions;

namespace BookShop.IntegrationTest.Application.Translator.Commands
{
    public class UpdateTranslatorCommandTest : TestBase
    {
        private readonly E.Translator _savedTranslator = new E.Translator()
        {
            Id = Guid.NewGuid(),
            CreateBy = string.Empty,
            CreateDate = DateTime.UtcNow,
            LastModifiedBy = string.Empty,
            LastModifiedDate = DateTime.UtcNow,
            Name = "test-translator",
            Gender = _randomizer.Enum<Gender>(),
            ImageName = string.Empty,
        };
        private UpdateTranslatorCommand updateTranslatorCommand = new UpdateTranslatorCommand
        {
            Name = _randomizer.String2(10),
            Gender = _randomizer.Enum<Gender>(),
        };
        private Result<Empty> result = new Result<Empty>();
        private async Task addTranslator()
        {
            await _TestDbContext.Add<E.Translator, Guid>(_savedTranslator);
        }
        private void assert_Translator_Updated(E.Translator? translator)
        {
            Assert.NotNull(translator);
            Assert.Equal(updateTranslatorCommand.Name, translator.Name);
            Assert.Equal(updateTranslatorCommand.Gender, translator.Gender);
        }
        private async Task requestAndGetResult()
        {
            result = await SendRequest<UpdateTranslatorCommand, Result<Empty>>(updateTranslatorCommand);
        }
        public UpdateTranslatorCommandTest(ApplicationCollectionFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            updateTranslatorCommand.Id = _savedTranslator.Id;
            addTranslator().GetAwaiter().GetResult();
            SetCurrentUser();
        }





        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            await _TestDbContext.SetPermissionForUser(PermissionConstants.UpdateTranslator);

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestDbContext.Get<E.Translator, Guid>(updateTranslatorCommand.Id);
            assert_Translator_Updated(entity);
        }

        
        [Fact]
        public async Task Valid_Request_With_ImageFile_ShouldReturn_SuccessResult_And_UpdateEntity_SaveImageFile()
        {
            //Arrnage 
            updateTranslatorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "translator-man.png"));
            await _TestDbContext.SetPermissionForUser(PermissionConstants.UpdateTranslator);
            int imagesCount = Directory.GetFiles(PathExtensions.Translator_Images).Count();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestDbContext.Get<E.Translator, Guid>(updateTranslatorCommand.Id);
            assert_Translator_Updated(entity);
            int actualImagesCount = Directory.GetFiles(PathExtensions.Translator_Images).Count();
            Assert.Equal(imagesCount + 1 , actualImagesCount);  
        }


        [Fact]
        public async Task When_Name_IsDuplicate_ShouldReturn_ValidationError()
        {
            //Arrnage 
            E.Translator translator = new E.Translator
            {
                Id = Guid.NewGuid(),
                CreateBy = string.Empty,
                CreateDate = DateTime.UtcNow,
                Name = $"translator-{Random.Shared.Next(1, 100)}",
            };
            await _TestDbContext.Add<E.Translator, Guid>(translator);
            updateTranslatorCommand.Name = translator.Name;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateTranslatorCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnauthorizeException()
        {
            //Arrange


            //Act
            var task = SendRequest<UpdateTranslatorCommand, Result<Empty>>(updateTranslatorCommand);

            //Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () =>
            {
                await task;
            });
        }


        [Fact]
        public async Task When_ImageFile_Size_GreaterThan_3MB_ShouldReturn_ValidationError()
        {
            //Arrange
            updateTranslatorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateTranslatorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_ImageFile_Extension_NotImage_ShouldReturn_ValidationError()
        {
            //Arrange
            updateTranslatorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateTranslatorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Name_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            updateTranslatorCommand.Name = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateTranslatorCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Name_Length_LessThan_3_ShouldReturn_ValidationError()
        {
            //Arrange
            updateTranslatorCommand.Name = "ab";

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateTranslatorCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Name_Length_GreaterThan_30_ShouldReturn_ValidationError()
        {
            //Arrange
            updateTranslatorCommand.Name = _randomizer.String2(31);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateTranslatorCommand.Name));
            _OutPutValidationErrors(result);
        }










    }
}
