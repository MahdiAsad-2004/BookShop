
using BookShop.Application.Extensions;
using BookShop.Application.Features.Author.Commands.Create;
using BookShop.Application.Features.Author.Commands.Update;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Exceptions;
using BookShop.IntegrationTest.Application.Common;
using Xunit.Abstractions;

namespace BookShop.IntegrationTest.Application.Author.Commands
{
    public class UpdateAuthorCommandTest : TestBase
    {
        E.Author _savedAuthor = new E.Author()
        {
            Id = Guid.NewGuid(),
            Name = "author-test",
            Gender = Domain.Enums.Gender.Man,
            CreateBy = string.Empty,
            CreateDate = DateTime.UtcNow,
            LastModifiedBy = string.Empty,
            LastModifiedDate = DateTime.UtcNow,
        };   
        UpdateAuthorCommand updateAuthorCommand = new UpdateAuthorCommand
        {
            Name = "authorUpdate-test",
            Gender = Domain.Enums.Gender.Woamn,
        };
        private async Task addAuthor()
        {
            await _TestDbContext.Add<E.Author, Guid>(_savedAuthor);
        }
        public UpdateAuthorCommandTest(ApplicationCollectionFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            updateAuthorCommand.Id = _savedAuthor.Id;
            addAuthor().GetAwaiter().GetResult();
            SetCurrentUser();
        }






        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            await _TestDbContext.SetPermissionForUser(PermissionConstants.UpdateAuthor);

            //Act
            var result = await SendRequest<UpdateAuthorCommand, Result<Empty>>(updateAuthorCommand);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestDbContext.Get<E.Author, Guid>(updateAuthorCommand.Id);
            Assert.NotNull(entity);
            Assert.Equal(entity.Name, updateAuthorCommand.Name);
            Assert.Equal(entity.Gender, updateAuthorCommand.Gender);
        }


        [Fact]
        public async Task Valid_Request_WithImageFile_ShouldReturn_SuccessResult_UpdateEntity_And_SaveNewImageFile()
        {
            //Arrnage 
            updateAuthorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "author-man.png"));
            await _TestDbContext.SetPermissionForUser(PermissionConstants.UpdateAuthor);
            int fileCounts = Directory.GetFiles(PathExtensions.Author_Images).Count();

            //Act
            var result = await SendRequest<UpdateAuthorCommand, Result<Empty>>(updateAuthorCommand);

            //Assert
            int actualFileCount = Directory.GetFiles(PathExtensions.Author_Images).Count();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(fileCounts + 1, actualFileCount);
            var entity = await _TestDbContext.Get<E.Author, Guid>(updateAuthorCommand.Id);
            Assert.NotNull(entity);
            Assert.Equal(entity.Name, updateAuthorCommand.Name);
            Assert.Equal(entity.Gender, updateAuthorCommand.Gender);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnAuthorizeException()
        {
            //Arrange
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddAuthor);

            //Act
            var task = SendRequest<UpdateAuthorCommand, Result<Empty>>(updateAuthorCommand);

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
                Gender = Domain.Enums.Gender.Man,
            };
            await _TestDbContext.Add<E.Author, Guid>(author);
            updateAuthorCommand.Name = author.Name;

            //Act
            var result = await SendRequest<UpdateAuthorCommand, Result<Empty>>(updateAuthorCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateAuthorCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileSize_ShouldReturn_ValidationError()
        {
            //Arrange
            updateAuthorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            var result = await SendRequest<UpdateAuthorCommand, Result<Empty>>(updateAuthorCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateAuthorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileExtension_ShouldReturn_ValidationError()
        {
            //Arrange
            updateAuthorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            var result = await SendRequest<UpdateAuthorCommand, Result<Empty>>(updateAuthorCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateAuthorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }





    }
}
