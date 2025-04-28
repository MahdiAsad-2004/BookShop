using BookShop.IntegrationTest.Features.Book.FakeData;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Application.Features.Book.Commands.Update;

namespace BookShop.IntegrationTest.Features.Book.Commands
{
    public class CreateBookCommandTest : TestFeatureBase
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
        private CreateBookCommand createBookCommand = new CreateBookCommand
        {
            Cover = Cover.ClothCover,
            Cutting = Cutting.B4,
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
            PublishYear = DateTime.UtcNow.AddYears(-5),
            WeightInGram = 500,
        };
        private Result<Empty> result = new Result<Empty>();
        public CreateBookCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            createBookCommand.PublisherId = publisher.Id;
            createBookCommand.AuthorIds = authors.Select(a => a.Id).ToArray();
            addRequiredEntities().GetAwaiter().GetResult();
        }
        private async Task addRequiredEntities()
        {
            await _TestRepository.Add<E.Publisher, Guid>(publisher);
            await _TestRepository.Add<E.Author, Guid>(authors);
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);
        }




        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_AddEntity_SaveImageFile()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.Book.Add);
            int booksCount = await _TestRepository.Count<E.Book, Guid>();
            int filesCount = Directory.GetFiles(PathExtensions.Product.Images).Count();

            //Act
            await requestAndGetResult();

            //Assert
            int newBooksCount = await _TestRepository.Count<E.Book, Guid>();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(booksCount + 1, newBooksCount);
            int actualFilesCount = Directory.GetFiles(PhysicalPath(PathExtensions.Product.Images)).Count();
            Assert.Equal(filesCount + 1, actualFilesCount);
        }


        [Fact]
        public async Task Valid_Request_WithPublisherId_ShouldReturn_SuccessResult()
        {
            //Arrnage 
            E.Publisher publisher = new E.Publisher
            {
                Id = Guid.NewGuid(),
                Title = _Randomizer.String2(20),
                ImageName = string.Empty,
            };
            await _TestRepository.Add<E.Publisher, Guid>(publisher);
            await _TestRepository.AddPermissionForUser(PermissionConstants.Book.Add);
            createBookCommand.PublisherId = publisher.Id;

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
                    Name = _Randomizer.String2(10),
                    Gender = Gender.Man,
                    ImageName = null,
                });
            };
            await _TestRepository.Add<E.Author, Guid>(authors);
            await _TestRepository.AddPermissionForUser(PermissionConstants.Book.Add);
            createBookCommand.AuthorIds = authors.Select(a => a.Id).ToArray();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task Valid_Request_WithTranslatorId_ShouldReturn_SuccessResult()
        {
            //Arrnage 
            E.Translator translator = new E.Translator
            {
                Id = Guid.NewGuid(),
                ImageName = string.Empty,
                Name = _Randomizer.String(10),
            };
            await _TestRepository.Add<E.Translator, Guid>(translator);
            await _TestRepository.AddPermissionForUser(PermissionConstants.Book.Add);
            createBookCommand.TranslatorId = translator.Id;

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnBookizeException()
        {
            //Arrange

            //Act
            var task = _TestRequestHandler.SendRequest<CreateBookCommand, Result<Empty>>(createBookCommand);

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
            E.Book book = BookFakeData.Create();
            book.Product.Title = $"book-{_Randomizer.Int(1, 100)}";
            await _TestRepository.Add<E.Book, Guid>(book);
            createBookCommand.Product_Title = book.Product.Title;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Title_Length_LessThan_3_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_Title = "ab";

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Title_Length_GreaterThan_50_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_Title = _Randomizer.String2(51);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_ImageFile_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_ImageFile = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_ImageFile_Size_GreaterThan_3Mb_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_ImageFile_Extension_NotImage_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.AuthorIds = [];

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Edition_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Edition = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Edition));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_NumberOfPages_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.NumberOfPages = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.NumberOfPages));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_DescriptionHtml_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_DescriptionHtml = null;

            //Act
            await requestAndGetResult();

            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_DescriptionHtml));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_DescriptionHtml_Length_GreaterThan_500_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_DescriptionHtml = _Randomizer.String2(505);

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_DescriptionHtml));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_NumberOfInventory_LessThan_0_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_NumberOfInventory = -1;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_NumberOfInventory));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Price_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_Price = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_Price));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_WeightInGram_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.WeightInGram = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.WeightInGram));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_WeightInGram_GreaterThan_10_000_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.WeightInGram = 10_002;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.WeightInGram));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_TranslatorId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.TranslatorId = Guid.Empty;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.TranslatorId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_TranslatorId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.TranslatorId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.TranslatorId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_CategoryId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_CategoryId = Guid.Empty;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_CategoryId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_CategoryId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.Product_CategoryId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.Product_CategoryId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_PublisherId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.PublisherId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.PublisherId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorIds_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.AuthorIds = [createBookCommand.AuthorIds[0], Guid.NewGuid()];

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorId_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.AuthorIds = null;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_PublishYear_GreaterThan_ToDate_ShouldReturn_ValidationError()
        {
            //Arrange
            createBookCommand.PublishYear = DateTime.UtcNow;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createBookCommand.PublishYear));
            _OutPutValidationErrors(result);
        }



























    }
}
