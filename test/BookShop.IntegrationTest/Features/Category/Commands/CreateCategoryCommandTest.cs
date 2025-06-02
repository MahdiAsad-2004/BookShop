using BookShop.Application.Features.Category.Commands.Create;
using BookShop.Application.Features.Translator.Commands.Create;

namespace BookShop.IntegrationTest.Features.Category.Commands
{
    public class CreateCategoryCommandTest : TestFeatureBase
    {
        CreateCategoryCommand createCategoryCommand = new CreateCategoryCommand
        {
            Title = "category-test",
            ParentId = null,
            ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "category.png")),
        };
        private Result<Empty> result = new Result<Empty>();
        public CreateCategoryCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
             
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<CreateCategoryCommand, Result<Empty>>(createCategoryCommand);
        }





        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_SaveImageFile()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.Categoory.Add);
            int fileCounts = Directory.GetFiles(PhysicalPath(PathExtensions.Category.Images)).Count();

            //Act
            await requestAndGetResult();

            //Assert
            int actualFileCount = Directory.GetFiles(PhysicalPath(PathExtensions.Category.Images)).Count();
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
            await _TestRepository.Add<E.Category, Guid>(parentCategory);
            await _TestRepository.AddPermissionForUser(PermissionConstants.Categoory.Add);
            createCategoryCommand.ParentId = parentCategory.Id;

            //Act
            await requestAndGetResult();

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
            await _TestRepository.Add<E.Category, Guid>(category);
            createCategoryCommand.Title = category.Title;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createCategoryCommand.Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnauthorizeException()
        {
            //Arrange


            //Act
            var task = _TestRequestHandler.SendRequest<CreateCategoryCommand, Result<Empty>>(createCategoryCommand);

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
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createCategoryCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileExtension_ShouldReturn_ValidationError()
        {
            //Arrange
            createCategoryCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createCategoryCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ParentId_ShouldReturn_ValidationError()
        {
            //Arrange
            createCategoryCommand.ParentId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createCategoryCommand.ParentId));
            _OutPutValidationErrors(result);
        }




    }
}
