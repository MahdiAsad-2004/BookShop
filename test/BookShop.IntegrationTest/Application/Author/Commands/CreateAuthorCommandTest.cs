
using BookShop.Application.Extensions;
using BookShop.Application.Features.Author.Commands.Create;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Exceptions;
using BookShop.IntegrationTest.Application.Common;
using Xunit.Abstractions;

namespace BookShop.IntegrationTest.Application.Author.Commands
{
    public class CreateAuthorCommandTest : TestBase
    {
        CreateAuthorCommand createAuthorCommand = new CreateAuthorCommand
        {
            Name = "author-test",
            Gender = Domain.Enums.Gender.Man,
        };
        public CreateAuthorCommandTest(ApplicationCollectionFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            SetCurrentUser();
        }


        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult()
        {
            //Arrnage 
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddAuthor);

            //Act
            var result = await SendRequest<CreateAuthorCommand, Result<Empty>>(createAuthorCommand);

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
            await _TestDbContext.Add<E.Author, Guid>(author);
            createAuthorCommand.Name = author.Name;

            //Act
            var result = await SendRequest<CreateAuthorCommand, Result<Empty>>(createAuthorCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createAuthorCommand.Name));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnAuthorizeException()
        {
            //Arrange


            //Act
            var task = SendRequest<CreateAuthorCommand, Result<Empty>>(createAuthorCommand);

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
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddAuthor);
            int fileCounts = Directory.GetFiles(PathExtensions.Author_Images).Count();

            //Act
            var result = await SendRequest<CreateAuthorCommand, Result<Empty>>(createAuthorCommand);

            //Assert
            int actualFileCount = Directory.GetFiles(PathExtensions.Author_Images).Count();
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
            var result = await SendRequest<CreateAuthorCommand, Result<Empty>>(createAuthorCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createAuthorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileExtension_ShouldReturn_ValidationError()
        {
            //Arrange
            createAuthorCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            var result = await SendRequest<CreateAuthorCommand, Result<Empty>>(createAuthorCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createAuthorCommand.ImageFile));
            _OutPutValidationErrors(result);
        }





    }
}
