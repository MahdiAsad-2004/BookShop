using BookShop.Application.Extensions;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Domain.Common;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Constants;
using BookShop.Domain.Exceptions;
using BookShop.IntegrationTest.Application.Common;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;

namespace BookShop.IntegrationTest.Application.Book.Commands
{
    public class CreateBookCommandTest : TestBase
    {
        private CreateBookCommand createBookCommand = new CreateBookCommand
        {
            Cover = Domain.Enums.Cover.ClothCover,
            Cutting = Domain.Enums.Cutting.B4,
            Language = Domain.Enums.Language.Persian,
            NumberOfPages = 1,
            Edition = null,
            Product_CategoryId = null,
            Product_DescriptionHtml = "",
            Product_NumberOfInventory = 1,
            Product_Price = 100_000,
            Product_Title = "ProductTite",
            Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "product.png"))
        };
        private E.Publisher publisher = new E.Publisher
        {
            Id = Guid.NewGuid(),
            Title = "PublisherTest",
            CreateDate = DateTime.Now,
            CreateBy = "",
            ImageName = "",
        };

        private List<E.Author> authors = new List<E.Author>
        {
            new E.Author
            {
                Id = Guid.NewGuid(),
                CreateBy = string.Empty,
                CreateDate = DateTime.UtcNow,
                Name = $"author-{Random.Shared.Next(1 , 100)}",
            },
            new E.Author
            {
                Id = Guid.NewGuid(),
                CreateBy = string.Empty,
                CreateDate = DateTime.UtcNow,
                Name = $"author-{Random.Shared.Next(1 , 100)}",
            }
        };

        public CreateBookCommandTest(ApplicationCollectionFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            createBookCommand.PublisherId = publisher.Id;
            createBookCommand.AuthorIds = authors.Select(a => a.Id).ToArray();
            AddRequiredEntities().GetAwaiter().GetResult();
            SetCurrentUser();
        }
        private async Task AddRequiredEntities()
        {
            await _TestDbContext.Add<E.Publisher, Guid>(publisher);
            await _TestDbContext.Add<E.Author, Guid>(authors);
        }




        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_SaveImageFile()
        {
            //Arrange
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddUser);
            var fileCounts = Directory.GetFiles(PathExtensions.Product_Images).Count();

            //Act
            var result = await SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);

            //Assert
            int actualFileCount = Directory.GetFiles(PathExtensions.Product_Images).Count();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(fileCounts + 1, actualFileCount);
        }


        [Fact]
        public async Task Invalid_CategoryId_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_CategoryId = Guid.NewGuid();

            //Act
            var result = await SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);

            //Asserrt
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createBookCommand.Product_CategoryId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_PublisherId_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.PublisherId = Guid.NewGuid();

            //Act
            var result = await SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);

            //Asserrt
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createBookCommand.PublisherId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnauthorizeException()
        {
            //Arrange


            //Act
            var task = SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);

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
            createBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            var result = await SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Invalid_ImageFileExtension_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            var result = await SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Duplicate_ProductTitle_ShouldReturn_ValidationError()
        {
            //Arrange
            E.Product product = new E.Product
            {
                CreateBy = string.Empty,
                CreateDate = DateTime.UtcNow,
                DescriptionHtml = string.Empty,
                Id = Guid.NewGuid(),
                LastModifiedBy = string.Empty,
                LastModifiedDate = DateTime.UtcNow,
                NumberOfInventory = 0,
                Price = 10_000,
                ProductType = Domain.Enums.ProductType.Book,
                SellCount = 0,
                Title = "Product-1234",
            };
            await _TestDbContext.Add<E.Product,Guid>(product);
            createBookCommand.Product_Title = product.Title;

            //Act
            var result = await SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }



        [Fact]
        public async Task Empty_AuthorIds_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.AuthorIds = [];

            //Act
            var result = await SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }
        
        

        [Fact]
        public async Task Invalid_AuthorIds_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.AuthorIds = [authors[0].Id, Guid.NewGuid()];

            //Act
            var result = await SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }
        







    }
}
