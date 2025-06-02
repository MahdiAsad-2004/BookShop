using BookShop.Application.Features.Author.Commands.Create;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Application.Features.EBook.Commands.Create;
using BookShop.Application.Features.Translator.Commands.Create;
using BookShop.Domain.Common;

namespace BookShop.IntegrationTest.Features.Author.Commands
{
    public class CreateAuthorCommandTest : TestFeatureBase
    {
        private Result<Empty> result = new Result<Empty>();
        CreateAuthorCommand createAuthorCommand = new CreateAuthorCommand
        {
            Name = "author-test",
            Gender = Gender.Man,
            ImageFile = null,
        };
        public CreateAuthorCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            // 
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<CreateAuthorCommand, Result<Empty>>(createAuthorCommand);
        }







        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.Author.Add);

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
            E.Author author = new E.Author
            {
                Id = Guid.NewGuid(),
                CreateBy = string.Empty,
                CreateDate = DateTime.UtcNow,
                Name = $"author-{Random.Shared.Next(1, 100)}",
            };
            await _TestRepository.Add<E.Author, Guid>(author);
            createAuthorCommand.Name = author.Name;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createAuthorCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnAuthorizeException()
        {
            //Arrange


            //Act
            var task = _TestRequestHandler.SendRequest<CreateAuthorCommand, Result<Empty>>(createAuthorCommand);

            //Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () =>
            {
                await task;
            });
        }


        [Fact]
        public async Task Valid_Request_WithImageFile_ShouldReturn_SuccessResult_And_SaveImageFile()
        {
            //Arrnage 
            createAuthorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "author-man.png"));
            await _TestRepository.AddPermissionForUser(PermissionConstants.Author.Add);
            int fileCounts = Directory.GetFiles(PhysicalPath(PathExtensions.Author.Images)).Count();

            //Act
            await requestAndGetResult();

            //Assert
            int actualFileCount = Directory.GetFiles(PhysicalPath(PathExtensions.Author.Images)).Count();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(fileCounts + 1, actualFileCount);
        }


        [Fact]
        public async Task Invalid_ImageFileSize_ShouldReturn_ValidationError()
        {
            //Arrange
            createAuthorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createAuthorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileExtension_ShouldReturn_ValidationError()
        {
            //Arrange
            createAuthorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createAuthorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }





    }
}
