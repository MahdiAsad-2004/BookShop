using BookShop.Application.Extensions;
using BookShop.Application.Features.Category.Commands.Create;
using BookShop.Application.Features.Category.Commands.Update;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Exceptions;
using BookShop.IntegrationTest.Application.Common;
using Xunit.Abstractions;

namespace BookShop.IntegrationTest.Application.Category.Commands
{
    public class UpdateDiscountCommandTest : TestBase
    {
        E.Category _savedCategory = new E.Category()
        {
            Id = Guid.NewGuid(),
            Title = "category-test",
            Parent = null,
            CreateBy = string.Empty,
            CreateDate = DateTime.UtcNow,
            LastModifiedBy = string.Empty,
            LastModifiedDate = DateTime.UtcNow,
            ImageName = null,
        };   
        UpdateCategoryCommand updateCategoryCommand = new UpdateCategoryCommand
        {
            Title = "categoryUpdate-test",
            ParentId = null,
        };
        private async Task addCategory()
        {
            await _TestDbContext.Add<E.Category, Guid>(_savedCategory);
        }
        public UpdateDiscountCommandTest(ApplicationCollectionFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            updateCategoryCommand.Id = _savedCategory.Id;
            addCategory().GetAwaiter().GetResult();
            SetCurrentUser();
        }






        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            await _TestDbContext.SetPermissionForUser(PermissionConstants.UpdateCategory);

            //Act
            var result = await SendRequest<UpdateCategoryCommand, Result<Empty>>(updateCategoryCommand);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestDbContext.Get<E.Category, Guid>(updateCategoryCommand.Id);
            Assert.NotNull(entity);
            Assert.Equal(entity.Title, updateCategoryCommand.Title);
            Assert.Equal(entity.ParentId, updateCategoryCommand.ParentId);
        }

        
        [Fact]
        public async Task Valid_Request_WithParentId_ShouldReturn_SuccessResult_And_UpdateEntity()
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
            await _TestDbContext.SetPermissionForUser(PermissionConstants.UpdateCategory);
            updateCategoryCommand.ParentId = parentCategory.Id;

            //Act
            var result = await SendRequest<UpdateCategoryCommand, Result<Empty>>(updateCategoryCommand);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestDbContext.Get<E.Category, Guid>(updateCategoryCommand.Id);
            Assert.NotNull(entity);
            Assert.Equal(entity.Title, updateCategoryCommand.Title);
            Assert.Equal(entity.ParentId, updateCategoryCommand.ParentId);
        }


        [Fact]
        public async Task Valid_Request_WithImageFile_ShouldReturn_SuccessResult_UpdateEntity_And_SaveNewImageFile()
        {
            //Arrnage 
            updateCategoryCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "category.png"));
            await _TestDbContext.SetPermissionForUser(PermissionConstants.UpdateCategory);
            int fileCounts = Directory.GetFiles(PathExtensions.Category_Images).Count();

            //Act
            var result = await SendRequest<UpdateCategoryCommand, Result<Empty>>(updateCategoryCommand);

            //Assert
            int actualFileCount = Directory.GetFiles(PathExtensions.Category_Images).Count();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(fileCounts + 1, actualFileCount);
            var entity = await _TestDbContext.Get<E.Category, Guid>(updateCategoryCommand.Id);
            Assert.NotNull(entity);
            Assert.Equal(entity.Title, updateCategoryCommand.Title);
            Assert.Equal(entity.ParentId, updateCategoryCommand.ParentId);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnCategoryizeException()
        {
            //Arrange
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddCategory);

            //Act
            var task = SendRequest<UpdateCategoryCommand, Result<Empty>>(updateCategoryCommand);

            //Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () =>
            {
                await task;
            });
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
                LastModifiedBy = string.Empty,
                LastModifiedDate = DateTime.UtcNow,
                Title = $"category-{Random.Shared.Next(1, 100)}",
                ParentId = null,
            };
            await _TestDbContext.Add<E.Category, Guid>(category);
            updateCategoryCommand.Title = category.Title;

            //Act
            var result = await SendRequest<UpdateCategoryCommand, Result<Empty>>(updateCategoryCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateCategoryCommand.Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileSize_ShouldReturn_ValidationError()
        {
            //Arrange
            updateCategoryCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            var result = await SendRequest<UpdateCategoryCommand, Result<Empty>>(updateCategoryCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateCategoryCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileExtension_ShouldReturn_ValidationError()
        {
            //Arrange
            updateCategoryCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            var result = await SendRequest<UpdateCategoryCommand, Result<Empty>>(updateCategoryCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateCategoryCommand.ImageFile));
            _OutPutValidationErrors(result);
        }





    }
}
