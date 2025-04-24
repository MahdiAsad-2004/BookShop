using BookShop.Application.Features.Translator.Commands.Create;

namespace BookShop.IntegrationTest.Features.Translator.Commands
{
    public class CreateTranslatorCommandTest : TestFeatureBase
    {
        CreateTranslatorCommand createTranslatorCommand = new CreateTranslatorCommand
        {
            Name = "translator-test",
            ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "translator-man.png")),
            Gender = _Randomizer.Enum<Gender>(),
        };
        private Result<Empty> result = new Result<Empty>();
        public CreateTranslatorCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
             
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<CreateTranslatorCommand, Result<Empty>>(createTranslatorCommand);
        }




        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_AddEntity_SaveImageFile()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.Translator.Add);
            int fileCounts = Directory.GetFiles(PathExtensions.Translator_Images).Count();
            int translatorsCount = await _TestRepository.Count<E.Translator, Guid>();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            int actualTranslatorsCount = await _TestRepository.Count<E.Translator, Guid>();
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
            await _TestRepository.Add<E.Translator, Guid>(translator);
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
            var task = _TestRequestHandler.SendRequest<CreateTranslatorCommand, Result<Empty>>(createTranslatorCommand);

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
            createTranslatorCommand.Name = _Randomizer.String2(31);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createTranslatorCommand.Name));
            _OutPutValidationErrors(result);
        }




    }
}
