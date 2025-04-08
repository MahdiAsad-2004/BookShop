
using BookShop.Application.Extensions;
using BookShop.Application.Features.Category.Commands.Create;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Exceptions;
using BookShop.IntegrationTest.Application.Common;
using Xunit.Abstractions;

namespace BookShop.IntegrationTest.Application.Category.Commands
{
    public class CreateCategoryCommandTest : TestBase
    {
        CreateCategoryCommand createCategoryCommand = new CreateCategoryCommand
        {
            Title = "category-test",
            ParentId = null,
            ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory() , "Files" , "category.png")),
        };
        public CreateCategoryCommandTest(ApplicationCollectionFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            SetCurrentUser();
        }



        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_SaveImageFile()
        {
            //Arrnage 
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddCategory);
            int fileCounts = Directory.GetFiles(PathExtensions.Category_Images).Count();

            //Act
            var result = await SendRequest<CreateCategoryCommand, Result<Empty>>(createCategoryCommand);

            //Assert
            int actualFileCount = Directory.GetFiles(PathExtensions.Category_Images).Count();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(fileCounts + 1, actualFileCount);
        }


        [Fact]
        public async Task Valid_Request_WithParentId_ShouldReturn_SuccessResult()
        {
            //Arrnage 
            E.Category parentCategory = new E.Category
            {
                Id = Guid.NewGuid(),
                CreateBy = string.Empty,
                CreateDate = DateTime.UtcNow,
                LastModifiedBy = string.Empty,
                LastModifiedDate = DateTime.UtcNow,
                Title = "parentCategory-test",
                ParentId = null,
                ImageName = null,
            };
            await _TestDbContext.Add<E.Category, Guid>(parentCategory);
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddCategory);
            createCategoryCommand.ParentId = parentCategory.Id;

            //Act
            var result = await SendRequest<CreateCategoryCommand, Result<Empty>>(createCategoryCommand);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task Duplicate_Title_ShouldReturn_ValidationError()
        {
            //Arrnage 
            E.Category category = new E.Category
            {
                Id = Guid.NewGuid(),
                CreateBy = string.Empty,
                CreateDate = DateTime.UtcNow,
                Title = $"category-{Random.Shared.Next(1, 100)}",
            };
            await _TestDbContext.Add<E.Category, Guid>(category);
            createCategoryCommand.Title = category.Title;

            //Act
            var result = await SendRequest<CreateCategoryCommand, Result<Empty>>(createCategoryCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createCategoryCommand.Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnauthorizeException()
        {
            //Arrange


            //Act
            var task = SendRequest<CreateCategoryCommand, Result<Empty>>(createCategoryCommand);

            //Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () =>
            {
                await task;
            });
        }


        [Fact]
        public async Task Invalid_ImageFileSize_ShouldReturn_ValidationError()
        {
            //Arrange
            createCategoryCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            var result = await SendRequest<CreateCategoryCommand, Result<Empty>>(createCategoryCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createCategoryCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileExtension_ShouldReturn_ValidationError()
        {
            //Arrange
            createCategoryCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            var result = await SendRequest<CreateCategoryCommand, Result<Empty>>(createCategoryCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createCategoryCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ParentId_ShouldReturn_ValidationError()
        {
            //Arrange
            createCategoryCommand.ParentId = Guid.NewGuid();

            //Act
            var result = await SendRequest<CreateCategoryCommand, Result<Empty>>(createCategoryCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createCategoryCommand.ParentId));
            _OutPutValidationErrors(result);
        }




    }
}
