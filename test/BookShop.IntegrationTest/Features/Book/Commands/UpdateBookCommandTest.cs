using BookShop.Application.Features.Book.Commands.Update;
using BookShop.Application.Features.Translator.Commands.Create;
using BookShop.Domain.Entities;
using BookShop.IntegrationTest.Common;
using BookShop.IntegrationTest.Features.Book.FakeData;

namespace BookShop.IntegrationTest.Features.Book.Commands
{
    public class UpdateBookCommandTest : TestFeatureBase
    {
        private readonly E.Book _savedBook = new E.Book()
        {
            Id = Guid.NewGuid(),
            CreateBy = string.Empty,
            CreateDate = DateTime.UtcNow,
            LastModifiedBy = string.Empty,
            LastModifiedDate = DateTime.UtcNow,
            Cover = Cover.ClothCover,
            Cutting = Cutting.B4,
            Language = Language.Persian,
            NumberOfPages = 1,
            Shabak = Guid.NewGuid().ToString().Substring(0, 12),
            Edition = null,
            TranslatorId = null,
            PublishYear = DateTime.UtcNow.AddYears(-5),
            WeightInGram = 500,
            Product = new E.Product
            {
                Id = Guid.NewGuid(),
                CategoryId = null,
                DescriptionHtml = "",
                NumberOfInventory = 1,
                Price = 100_000,
                Title = "ProductTite",
            },
            Publisher = new E.Publisher
            {
                Id = Guid.NewGuid(),
                Title = "PublisherTest",
                CreateDate = DateTime.Now,
                CreateBy = "",
                ImageName = "",
            },
            Author_Books = new List<E.Author_Book>
            {
                new E.Author_Book
                {
                    Id = Guid.NewGuid(),
                    Author = new E.Author
                    {
                        Id = Guid.NewGuid(),
                        CreateBy = string.Empty,
                        CreateDate = DateTime.UtcNow,
                        Name = $"author-{Random.Shared.Next(1 , 100)}",
                    }
                },
                new E.Author_Book
                {
                    Id = Guid.NewGuid(),
                    Author = new E.Author
                    {
                        Id = Guid.NewGuid(),
                        CreateBy = string.Empty,
                        CreateDate = DateTime.UtcNow,
                        Name = $"author-{Random.Shared.Next(1 , 100)}",
                    }
                }
            }
        };
        private UpdateBookCommand updateBookCommand = new UpdateBookCommand
        {
            Cover = _Randomizer.Enum<Cover>(),
            Cutting = _Randomizer.Enum<Cutting>(),
            Language = _Randomizer.Enum<Language>(),
            NumberOfPages = _Randomizer.Int(1, 300),
            Edition = _Randomizer.Int(1, 5),
            TranslatorId = null,
            PublishYear = DateTime.UtcNow.AddDays(_Randomizer.Int(-100, -5)),
            WeightInGram = _Randomizer.Int(1, 1_000),
            Product_CategoryId = null,
            Product_DescriptionHtml = "",
            Product_NumberOfInventory = _Randomizer.Int(1, 100),
            Product_Price = _Randomizer.Int(10, 100_000),
            Product_Title = _Randomizer.String2(20),
        };
        private Result<Empty> result = new Result<Empty>();
        public UpdateBookCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            updateBookCommand.Id = _savedBook.Id;
            updateBookCommand.AuthorIds = _savedBook.Author_Books.Select(a => a.Author).Select(a => a.Id).ToArray();
            updateBookCommand.PublisherId = _savedBook.Publisher.Id;
            addBook().GetAwaiter().GetResult();
        }
        private async Task addBook()
        {
            await _TestRepository.Add<E.Book, Guid>(_savedBook);
        }
        private void assert_Book_And_Product_Updated(E.Book? book, E.Product? product, List<Author_Book>? author_Books)
        {
            Assert.NotNull(book);
            Assert.NotNull(product);
            Assert.Equal(updateBookCommand.Cover, book.Cover);
            Assert.Equal(updateBookCommand.Cutting, book.Cutting);
            Assert.Equal(updateBookCommand.Edition, book.Edition);
            Assert.Equal(updateBookCommand.Language, book.Language);
            Assert.Equal(updateBookCommand.NumberOfPages, book.NumberOfPages);
            Assert.Equal(updateBookCommand.Product_CategoryId, product.CategoryId);
            Assert.Equal(updateBookCommand.Product_DescriptionHtml, product.DescriptionHtml);
            Assert.Equal(updateBookCommand.Product_NumberOfInventory, product.NumberOfInventory);
            Assert.Equal(updateBookCommand.Product_Price, product.Price);
            Assert.Equal(updateBookCommand.Product_Title, product.Title);
            Assert.Equal(updateBookCommand.PublisherId, book.PublisherId);
            Assert.Equal(updateBookCommand.PublishYear, book.PublishYear);
            Assert.Equal(updateBookCommand.TranslatorId, book.TranslatorId);
            Assert.Equal(updateBookCommand.WeightInGram, book.WeightInGram);
            if (author_Books != null)
            {
                var expectedAuthorIds = updateBookCommand.AuthorIds.ToList();
                var actualAuthorIds = author_Books.Select(a => a.AuthorId).ToList();
                Assert.Equal(expectedAuthorIds.Count, actualAuthorIds.Count);
                foreach (Guid authorId in expectedAuthorIds)
                {
                    Assert.Contains(actualAuthorIds, a => a == authorId);
                }
            }
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<UpdateBookCommand, Result<Empty>>(updateBookCommand);
        }





        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.Book.Update);

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.Book, Guid>(updateBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedBook.Product.Id);
            assert_Book_And_Product_Updated(entity, entityProduct, null);
        }


        [Fact]
        public async Task Valid_Request_WithPublisherId_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            E.Publisher publisher = new E.Publisher
            {
                Id = Guid.NewGuid(),
                Title = _Randomizer.String2(20),
                ImageName = string.Empty,
            };
            await _TestRepository.Add<E.Publisher, Guid>(publisher);
            await _TestRepository.AddPermissionForUser(PermissionConstants.Book.Update);
            updateBookCommand.PublisherId = publisher.Id;

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.Book, Guid>(updateBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedBook.Product.Id);
            assert_Book_And_Product_Updated(entity, entityProduct, null);
        }


        [Fact]
        public async Task Valid_Request_WithAuthorIds_ShouldReturn_SuccessResult_And_UpdateEntity()
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
            await _TestRepository.AddPermissionForUser(PermissionConstants.Book.Update);
            Guid[] authorIds = [.. authors.Select(a => a.Id).ToArray(), _savedBook.Author_Books.Select(a => a.Author).ToList()[0].Id];
            updateBookCommand.AuthorIds = authorIds;


            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.Book, Guid>(updateBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedBook.Product.Id);
            var entityAuthor_books = await _TestRepository.GetAll<E.Author_Book, Guid>(a => a.BookId == updateBookCommand.Id);
            assert_Book_And_Product_Updated(entity, entityProduct, entityAuthor_books);
        }


        [Fact]
        public async Task Valid_Request_WithTranslatorId_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            E.Translator translator = new E.Translator
            {
                Id = Guid.NewGuid(),
                ImageName = string.Empty,
                Name = _Randomizer.String(10),
            };
            await _TestRepository.Add<E.Translator, Guid>(translator);
            await _TestRepository.AddPermissionForUser(PermissionConstants.Book.Update);
            updateBookCommand.TranslatorId = translator.Id;

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.Book, Guid>(updateBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedBook.Product.Id);
            assert_Book_And_Product_Updated(entity, entityProduct, null);
        }


        [Fact]
        public async Task Valid_Request_WithImageFile_ShouldReturn_SuccessResult_UpdateEntity_And_SaveNewImageFile()
        {
            //Arrnage 
            updateBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "book.png"));
            await _TestRepository.AddPermissionForUser(PermissionConstants.Book.Update);
            int filesCount = Directory.GetFiles(PathExtensions.Product_Images).Count();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            int actualFilesCount = Directory.GetFiles(PathExtensions.Product_Images).Count();
            Assert.Equal(filesCount + 1, actualFilesCount);
            var entity = await _TestRepository.Get<E.Book, Guid>(updateBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedBook.Product.Id);
            assert_Book_And_Product_Updated(entity, entityProduct, null);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnBookizeException()
        {
            //Arrange


            //Act
            var task = _TestRequestHandler.SendRequest<UpdateBookCommand, Result<Empty>>(updateBookCommand);

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
            updateBookCommand.Product_Title = book.Product.Title;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Title_Length_LessThan_3_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Product_Title = "ab";

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Title_Length_GreaterThan_50_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Product_Title = _Randomizer.String2(51);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_ImageFile_Size_GreaterThan_3Mb_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_ImageFile_Extension_NotImage_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorId_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.AuthorIds = null;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.AuthorIds = [];

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Edition_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Edition = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Edition));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_NumberOfPages_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.NumberOfPages = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.NumberOfPages));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_DescriptionHtml_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Product_DescriptionHtml = null;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateBookCommand.Product_DescriptionHtml));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_DescriptionHtml_Length_GreaterThan_500_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Product_DescriptionHtml = _Randomizer.String2(505);

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Product_DescriptionHtml));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_NumberOfInventory_LessThan_0_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Product_NumberOfInventory = -1;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Product_NumberOfInventory));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Price_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Product_Price = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Product_Price));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_WeightInGram_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.WeightInGram = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.WeightInGram));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_WeightInGram_GreaterThan_10_000_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.WeightInGram = 10_002;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateBookCommand.WeightInGram));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_TranslatorId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.TranslatorId = Guid.Empty;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.TranslatorId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_TranslatorId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.TranslatorId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.TranslatorId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_CategoryId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Product_CategoryId = Guid.Empty;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Product_CategoryId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_CategoryId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.Product_CategoryId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.Product_CategoryId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_PublisherId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.PublisherId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.PublisherId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorIds_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            updateBookCommand.AuthorIds = [updateBookCommand.AuthorIds[0], Guid.NewGuid()];

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updateBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }

































    }
}
