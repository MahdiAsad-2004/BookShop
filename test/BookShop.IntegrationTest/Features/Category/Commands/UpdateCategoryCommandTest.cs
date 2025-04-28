using BookShop.Application.Features.Category.Commands.Update;
using BookShop.Application.Features.Discount.Commands.Create;
using BookShop.Application.Features.Translator.Commands.Create;
using BookShop.Domain.Common;

namespace BookShop.IntegrationTest.Features.Category.Commands
{
    public class UpdateCategoryCommandTest : TestFeatureBase
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
        private Result<Empty> result = new Result<Empty>();
        public UpdateCategoryCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            updateCategoryCommand.Id = _savedCategory.Id;
            addCategory().GetAwaiter().GetResult();
        }
        private async Task addCategory()
        {
            await _TestRepository.Add<E.Category, Guid>(_savedCategory);
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<UpdateCategoryCommand, Result<Empty>>(updateCategoryCommand);
        }





        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.Categoory.Update);

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.Category, Guid>(updateCategoryCommand.Id);
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
            await _TestRepository.Add<E.Category, Guid>(parentCategory);
            await _TestRepository.AddPermissionForUser(PermissionConstants.Categoory.Update);
            updateCategoryCommand.ParentId = parentCategory.Id;

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.Category, Guid>(updateCategoryCommand.Id);
            Assert.NotNull(entity);
            Assert.Equal(entity.Title, updateCategoryCommand.Title);
            Assert.Equal(entity.ParentId, updateCategoryCommand.ParentId);
        }


        [Fact]
        public async Task Valid_Request_WithImageFile_ShouldReturn_SuccessResult_UpdateEntity_And_SaveNewImageFile()
        {
            //Arrnage 
            updateCategoryCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "category.png"));
            await _TestRepository.AddPermissionForUser(PermissionConstants.Categoory.Update);
            int fileCounts = Directory.GetFiles(PathExtensions.Category.Images).Count();

            //Act
            await requestAndGetResult();

            //Assert
            int actualFileCount = Directory.GetFiles(PathExtensions.Category.Images).Count();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(fileCounts + 1, actualFileCount);
            var entity = await _TestRepository.Get<E.Category, Guid>(updateCategoryCommand.Id);
            Assert.NotNull(entity);
            Assert.Equal(entity.Title, updateCategoryCommand.Title);
            Assert.Equal(entity.ParentId, updateCategoryCommand.ParentId);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnCategoryizeException()
        {
            //Arrange


            //Act
            var task = _TestRequestHandler.SendRequest<UpdateCategoryCommand, Result<Empty>>(updateCategoryCommand);

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
            await _TestRepository.Add<E.Category, Guid>(category);
            updateCategoryCommand.Title = category.Title;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateCategoryCommand.Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileSize_ShouldReturn_ValidationError()
        {
            //Arrange
            updateCategoryCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateCategoryCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileExtension_ShouldReturn_ValidationError()
        {
            //Arrange
            updateCategoryCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateCategoryCommand.ImageFile));
            _OutPutValidationErrors(result);
        }





    }
}
