using BookShop.Application.Features.Author.Commands.Create;
using BookShop.Application.Features.Author.Commands.Update;
using BookShop.Domain.Common;

namespace BookShop.IntegrationTest.Features.Author.Commands
{
    public class UpdateAuthorCommandTest : TestFeatureBase
    {
        E.Author _savedAuthor = new E.Author()
        {
            Id = Guid.NewGuid(),
            Name = "author-test",
            Gender = Gender.Man,
            CreateBy = string.Empty,
            CreateDate = DateTime.UtcNow,
            LastModifiedBy = string.Empty,
            LastModifiedDate = DateTime.UtcNow,
        };
        UpdateAuthorCommand updateAuthorCommand = new UpdateAuthorCommand
        {
            Name = "authorUpdate-test",
            Gender = Gender.Woamn,
        };
        private Result<Empty> result = new Result<Empty>();
        public UpdateAuthorCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            updateAuthorCommand.Id = _savedAuthor.Id;
            addAuthor().GetAwaiter().GetResult();
        }
        private async Task addAuthor()
        {
            await _TestRepository.Add<E.Author, Guid>(_savedAuthor);
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<UpdateAuthorCommand, Result<Empty>>(updateAuthorCommand);
        }





        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.Author.Update);

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.Author, Guid>(updateAuthorCommand.Id);
            Assert.NotNull(entity);
            Assert.Equal(entity.Name, updateAuthorCommand.Name);
            Assert.Equal(entity.Gender, updateAuthorCommand.Gender);
        }


        [Fact]
        public async Task Valid_Request_WithImageFile_ShouldReturn_SuccessResult_UpdateEntity_And_SaveNewImageFile()
        {
            //Arrnage 
            updateAuthorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "author-man.png"));
            await _TestRepository.AddPermissionForUser(PermissionConstants.Author.Update);
            int fileCounts = Directory.GetFiles(PathExtensions.Author.Images).Count();

            //Act
            await requestAndGetResult();

            //Assert
            int actualFileCount = Directory.GetFiles(PathExtensions.Author.Images).Count();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(fileCounts + 1, actualFileCount);
            var entity = await _TestRepository.Get<E.Author, Guid>(updateAuthorCommand.Id);
            Assert.NotNull(entity);
            Assert.Equal(entity.Name, updateAuthorCommand.Name);
            Assert.Equal(entity.Gender, updateAuthorCommand.Gender);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnAuthorizeException()
        {
            //Arrange
            await _TestRepository.AddPermissionForUser(PermissionConstants.Author.Add);

            //Act
            var task = _TestRequestHandler.SendRequest<UpdateAuthorCommand, Result<Empty>>(updateAuthorCommand);

            //Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () =>
            {
                await task;
            });
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
                Gender = Gender.Man,
            };
            await _TestRepository.Add<E.Author, Guid>(author);
            updateAuthorCommand.Name = author.Name;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateAuthorCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileSize_ShouldReturn_ValidationError()
        {
            //Arrange
            updateAuthorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateAuthorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileExtension_ShouldReturn_ValidationError()
        {
            //Arrange
            updateAuthorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateAuthorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }





    }
}
