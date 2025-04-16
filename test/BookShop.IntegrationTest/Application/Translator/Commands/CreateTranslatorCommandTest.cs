using BookShop.Application.Extensions;
using BookShop.Application.Features.Translator.Commands.Create;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Exceptions;
using BookShop.IntegrationTest.Application.Common;
using Xunit.Abstractions;
using BookShop.Domain.Enums;

namespace BookShop.IntegrationTest.Application.Translator.Commands
{
    public class CreateTranslatorCommandTest : TestBase
    {
        CreateTranslatorCommand createTranslatorCommand = new CreateTranslatorCommand
        {
            Name = "translator-test",
            ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory() , "Files" , "translator-man.png")),
            Gender = _randomizer.Enum<Gender>(),
        };
        private Result<Empty> result = new Result<Empty>();
        private async Task requestAndGetResult()
        {
            result = await SendRequest<CreateTranslatorCommand, Result<Empty>>(createTranslatorCommand);
        }
        public CreateTranslatorCommandTest(ApplicationCollectionFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            SetCurrentUser();
        }




        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_AddEntity_SaveImageFile()
        {
            //Arrnage 
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddTranslator);
            int fileCounts = Directory.GetFiles(PathExtensions.Translator_Images).Count();
            int translatorsCount = await _TestDbContext.Count<E.Translator , Guid>();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            int actualTranslatorsCount = await _TestDbContext.Count<E.Translator, Guid>();
            Assert.Equal(translatorsCount + 1, actualTranslatorsCount);
            int actualFileCount = Directory.GetFiles(PathExtensions.Translator_Images).Count();
            Assert.Equal(fileCounts + 1, actualFileCount);
        }


        [Fact]
        public async Task When_Name_IsDuplicate_ShouldReturn_ValidationError()
        {
            //Arrnage 
            E.Translator translator = new E.Translator
            {
                Id = Guid.NewGuid(),
                Name = $"translator-{Random.Shared.Next(1, 100)}",
            };
            await _TestDbContext.Add<E.Translator, Guid>(translator);
            createTranslatorCommand.Name = translator.Name;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createTranslatorCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnauthorizeException()
        {
            //Arrange


            //Act
            var task = SendRequest<CreateTranslatorCommand, Result<Empty>>(createTranslatorCommand);

            //Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () =>
            {
                await task;
            });
        }


        [Fact]
        public async Task When_ImageFile_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createTranslatorCommand.ImageFile = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createTranslatorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_ImageFile_Size_GreaterThan_3MB_ShouldReturn_ValidationError()
        {
            //Arrange
            createTranslatorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createTranslatorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_ImageFile_Extension_NotImage_ShouldReturn_ValidationError()
        {
            //Arrange
            createTranslatorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createTranslatorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Name_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createTranslatorCommand.Name = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createTranslatorCommand.Name));
            _OutPutValidationErrors(result);
        }
     
        
        [Fact]
        public async Task When_Name_Length_LessThan_3_ShouldReturn_ValidationError()
        {
            //Arrange
            createTranslatorCommand.Name = "ab";

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createTranslatorCommand.Name));
            _OutPutValidationErrors(result);
        }
     

        [Fact]
        public async Task When_Name_Length_GreaterThan_30_ShouldReturn_ValidationError()
        {
            //Arrange
            createTranslatorCommand.Name = _randomizer.String2(31);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createTranslatorCommand.Name));
            _OutPutValidationErrors(result);
        }
     



    }
}
