using BookShop.Application.Extensions;
using BookShop.Application.Features.EBook.Commands.Create;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using BookShop.IntegrationTest.Application.Common;
using BookShop.IntegrationTest.Application.EBook.FakeData;
using Xunit.Abstractions;

namespace BookShop.IntegrationTest.Application.EBook.Commands
{
    public class CreateEBookCommandTest : TestBase
    {
        private readonly List<E.Author> authors = new List<E.Author>
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
        private readonly E.Publisher publisher = new E.Publisher
        {
            Id = Guid.NewGuid(),
            Title = "PublisherTest",
            CreateDate = DateTime.Now,
            CreateBy = "",
            ImageName = "",
        };
        private CreateEBookCommand createEBookCommand = new CreateEBookCommand
        {
            Language = Language.Persian,
            NumberOfPages = 1,
            Edition = null,
            Product_CategoryId = null,
            Product_DescriptionHtml = "",
            Product_NumberOfInventory = 1,
            Product_Price = 100_000,
            Product_Title = "ProductTite",
            Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "product.png")),
            TranslatorId = null,
            PublishYear = DateTime.UtcNow.AddYears(- 5),
            EBookFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "ebook-file.pdf")),
        };
        private Result<Empty> result = new Result<Empty>();
        private async Task addRequiredEntities()
        {
            await _TestDbContext.Add<E.Publisher, Guid>(publisher);
            await _TestDbContext.Add<E.Author, Guid>(authors);
        }
        private async Task requestAndGetResult()
        {
            result = await SendRequest<CreateEBookCommand, Result<Empty>>(createEBookCommand);
        }
        public CreateEBookCommandTest(ApplicationCollectionFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            createEBookCommand.PublisherId = publisher.Id;
            createEBookCommand.AuthorIds = authors.Select(a => a.Id).ToArray();
            addRequiredEntities().GetAwaiter().GetResult();
            SetCurrentUser();
        }




        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_AddEntity_SaveEbookFile_SaveImageFile()
        {
            //Arrnage 
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddEBook);
            int booksCount = await _TestDbContext.Count<E.EBook, Guid>();
            int filesCount = Directory.GetFiles(PathExtensions.EBook_Files).Count();
            int imagesCount = Directory.GetFiles(PathExtensions.Product_Images).Count();

            //Act
            await requestAndGetResult();

            //Assert
            int newEBooksCount = await _TestDbContext.Count<E.EBook, Guid>();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(booksCount + 1, newEBooksCount);
            int actualFilesCount = Directory.GetFiles(PathExtensions.EBook_Files).Count();
            int actualImagesCount = Directory.GetFiles(PathExtensions.Product_Images).Count();
            Assert.Equal(filesCount + 1, actualFilesCount);
            Assert.Equal(imagesCount + 1, actualImagesCount);
        }


        [Fact]
        public async Task Valid_Request_WithPublisherId_ShouldReturn_SuccessResult()
        {
            //Arrnage 
            E.Publisher publisher = new E.Publisher
            {
                Id = Guid.NewGuid(),
                Title = _randomizer.String2(20),
                ImageName = string.Empty,
            };
            await _TestDbContext.Add<E.Publisher, Guid>(publisher);
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddEBook);
            createEBookCommand.PublisherId = publisher.Id;

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task Valid_Request_WithAuthorIds_ShouldReturn_SuccessResult()
        {
            //Arrnage 
            List<E.Author> authors = new List<E.Author>();
            for (int i = 0; i < Random.Shared.Next(1, 4); i++)
            {
                authors.Add(new E.Author
                {
                    Id = Guid.NewGuid(),
                    Name = _randomizer.String2(10),
                    Gender = Gender.Man,
                    ImageName = null,
                });
            };
            await _TestDbContext.Add<E.Author, Guid>(authors);
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddEBook);
            createEBookCommand.AuthorIds = authors.Select(a => a.Id).ToArray();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task Valid_Request_WithTranslatorId_ShouldReturn_SuccessResult_And()
        {
            //Arrnage 
            E.Translator translator = new E.Translator
            {
                Id = Guid.NewGuid(),
                ImageName = string.Empty,
                Name = _randomizer.String(10),
            };
            await _TestDbContext.Add<E.Translator, Guid>(translator);
            await _TestDbContext.SetPermissionForUser(PermissionConstants.AddEBook);
            createEBookCommand.TranslatorId = translator.Id;

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnEBookizeException()
        {
            //Arrange


            //Act
            var task = SendRequest<CreateEBookCommand, Result<Empty>>(createEBookCommand);

            //Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () =>
            {
                await task;
            });
        }


        [Fact]
        public async Task When_Product_Title_IsDuplicate_ShouldReturn_ValidationError()
        {
            //Arrnage 
            E.EBook book = EBookFakeData.Create();
            book.Product.Title = $"book-{_randomizer.Int(1, 100)}";
            await _TestDbContext.Add<E.EBook, Guid>(book);
            createEBookCommand.Product_Title = book.Product.Title;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Title_Length_LessThan_3_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_Title = "ab";

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Title_Length_GreaterThan_50_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_Title = _randomizer.String2(51);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_ImageFile_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_ImageFile = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_ImageFile_Size_GreaterThan_3Mb_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_ImageFile_Extension_NotImage_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.AuthorIds = [];

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Edition_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Edition = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Edition));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_NumberOfPages_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.NumberOfPages = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.NumberOfPages));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_DescriptionHtml_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_DescriptionHtml = null;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_DescriptionHtml));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_DescriptionHtml_Length_GreaterThan_500_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_DescriptionHtml = _randomizer.String2(505);

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_DescriptionHtml));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_NumberOfInventory_LessThan_0_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_NumberOfInventory = -1;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_NumberOfInventory));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Price_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_Price = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_Price));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_TranslatorId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.TranslatorId = Guid.Empty;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.TranslatorId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_TranslatorId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.TranslatorId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.TranslatorId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_CategoryId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_CategoryId = Guid.Empty;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_CategoryId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_CategoryId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.Product_CategoryId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.Product_CategoryId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_PublisherId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.PublisherId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.PublisherId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorIds_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.AuthorIds = [createEBookCommand.AuthorIds[0], Guid.NewGuid()];

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorId_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.AuthorIds = null;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_EBookFile_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.EBookFile = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.EBookFile));
            _OutPutValidationErrors(result);

        }

        
        [Fact]
        public async Task When_EBookFile_Extension_NotAllowed_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.EBookFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(createEBookCommand.EBookFile));
            _OutPutValidationErrors(result);

        }


        [Fact]
        public async Task When_Publishear_GreaterThan_ToDate_ShouldReturn_ValidationError()
        {
            //Arrange
            createEBookCommand.PublishYear = DateTime.UtcNow;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createEBookCommand.PublishYear));
            _OutPutValidationErrors(result);
        }



















    }
}
