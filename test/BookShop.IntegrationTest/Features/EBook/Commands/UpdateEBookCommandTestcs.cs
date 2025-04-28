using BookShop.Application.Features.EBook.Commands.Update;
using BookShop.IntegrationTest.Features.EBook.FakeData;

namespace BookShop.IntegrationTest.Features.EBook.Commands
{
    public class UpdateEBookCommandTest : TestFeatureBase
    {
        private readonly E.EBook _savedEBook = new E.EBook()
        {
            Id = Guid.NewGuid(),
            CreateBy = string.Empty,
            CreateDate = DateTime.UtcNow,
            LastModifiedBy = string.Empty,
            LastModifiedDate = DateTime.UtcNow,
            Language = Language.Persian,
            NumberOfPages = 1,
            Edition = null,
            TranslatorId = null,
            PublishYear = DateTime.UtcNow.AddYears(-5),
            FileFormat = EBookFileFormat.Pdf,
            FileName = string.Empty,
            FileSize_KB = 1_000,
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
            Author_EBooks = new List<E.Author_EBook>
            {
                new E.Author_EBook
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
                new E.Author_EBook
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
        private UpdateEBookCommand updateEBookCommand = new UpdateEBookCommand
        {
            Language = _Randomizer.Enum<Language>(),
            NumberOfPages = _Randomizer.Int(1, 300),
            Edition = _Randomizer.Int(1, 5),
            TranslatorId = null,
            PublishYear = DateTime.UtcNow.AddDays(_Randomizer.Int(-100, -5)),
            Product_CategoryId = null,
            Product_DescriptionHtml = "",
            Product_NumberOfInventory = _Randomizer.Int(1, 100),
            Product_Price = _Randomizer.Int(10, 100_000),
            Product_Title = _Randomizer.String2(20),
        };
        private Result<Empty> result = new Result<Empty>();
        public UpdateEBookCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            updateEBookCommand.Id = _savedEBook.Id;
            updateEBookCommand.AuthorIds = _savedEBook.Author_EBooks.Select(a => a.Author).Select(a => a.Id).ToArray();
            updateEBookCommand.PublisherId = _savedEBook.Publisher.Id;
            addEBook().GetAwaiter().GetResult();
             
        }
        private async Task addEBook()
        {
            await _TestRepository.Add<E.EBook, Guid>(_savedEBook);
        }
        private void assert_EBook_And_Product_Updated(E.EBook? book, E.Product? product, List<E.Author_EBook>? author_EBooks)
        {
            Assert.NotNull(book);
            Assert.NotNull(product);
            Assert.Equal(updateEBookCommand.Edition, book.Edition);
            Assert.Equal(updateEBookCommand.Language, book.Language);
            Assert.Equal(updateEBookCommand.NumberOfPages, book.NumberOfPages);
            Assert.Equal(updateEBookCommand.Product_CategoryId, product.CategoryId);
            Assert.Equal(updateEBookCommand.Product_DescriptionHtml, product.DescriptionHtml);
            Assert.Equal(updateEBookCommand.Product_NumberOfInventory, product.NumberOfInventory);
            Assert.Equal(updateEBookCommand.Product_Price, product.Price);
            Assert.Equal(updateEBookCommand.Product_Title, product.Title);
            Assert.Equal(updateEBookCommand.PublisherId, book.PublisherId);
            Assert.Equal(updateEBookCommand.PublishYear, book.PublishYear);
            Assert.Equal(updateEBookCommand.TranslatorId, book.TranslatorId);
            if (author_EBooks != null)
            {
                var expectedAuthorIds = updateEBookCommand.AuthorIds.ToList();
                var actualAuthorIds = author_EBooks.Select(a => a.AuthorId).ToList();
                Assert.Equal(expectedAuthorIds.Count, actualAuthorIds.Count);
                foreach (Guid authorId in expectedAuthorIds)
                {
                    Assert.Contains(actualAuthorIds, a => a == authorId);
                }
            }
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<UpdateEBookCommand, Result<Empty>>(updateEBookCommand);
        }






        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.EBook.Update);

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.EBook, Guid>(updateEBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedEBook.Product.Id);
            assert_EBook_And_Product_Updated(entity, entityProduct, null);
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
            await _TestRepository.AddPermissionForUser(PermissionConstants.EBook.Update);
            updateEBookCommand.PublisherId = publisher.Id;

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.EBook, Guid>(updateEBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedEBook.Product.Id);
            assert_EBook_And_Product_Updated(entity, entityProduct, null);
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
            await _TestRepository.AddPermissionForUser(PermissionConstants.EBook.Update);
            Guid[] authorIds = [.. authors.Select(a => a.Id).ToArray(), _savedEBook.Author_EBooks.Select(a => a.Author).ToList()[0].Id];
            updateEBookCommand.AuthorIds = authorIds;


            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.EBook, Guid>(updateEBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedEBook.Product.Id);
            var entityAuthor_books = await _TestRepository.GetAll<E.Author_EBook, Guid>(a => a.EBookId == updateEBookCommand.Id);
            assert_EBook_And_Product_Updated(entity, entityProduct, entityAuthor_books);
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
            await _TestRepository.AddPermissionForUser(PermissionConstants.EBook.Update);
            updateEBookCommand.TranslatorId = translator.Id;

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.EBook, Guid>(updateEBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedEBook.Product.Id);
            assert_EBook_And_Product_Updated(entity, entityProduct, null);
        }


        [Fact]
        public async Task Valid_Request_WithEBookFile_ShouldReturn_SuccessResult_UpdateEntity_SaveNewEBookFile()
        {
            //Arrnage 
            updateEBookCommand.EBookFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "ebook-file.pdf"));
            await _TestRepository.AddPermissionForUser(PermissionConstants.EBook.Update);
            int filesCount = Directory.GetFiles(PathExtensions.EBook.Files).Count();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            int actualFilesCount = Directory.GetFiles(PathExtensions.EBook.Files).Count();
            Assert.Equal(filesCount + 1, actualFilesCount);
            var entity = await _TestRepository.Get<E.EBook, Guid>(updateEBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedEBook.Product.Id);
            assert_EBook_And_Product_Updated(entity, entityProduct, null);
        }


        [Fact]
        public async Task Valid_Request_WithImageFile_ShouldReturn_SuccessResult_UpdateEntity_SaveNewImageFile()
        {
            //Arrnage 
            updateEBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "ebook.png"));
            await _TestRepository.AddPermissionForUser(PermissionConstants.EBook.Update);
            int filesCount = Directory.GetFiles(PathExtensions.Product.Images).Count();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            int actualFilesCount = Directory.GetFiles(PathExtensions.Product.Images).Count();
            Assert.Equal(filesCount + 1, actualFilesCount);
            var entity = await _TestRepository.Get<E.EBook, Guid>(updateEBookCommand.Id);
            var entityProduct = await _TestRepository.Get<E.Product, Guid>(_savedEBook.Product.Id);
            assert_EBook_And_Product_Updated(entity, entityProduct, null);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnEBookizeException()
        {
            //Arrange


            //Act
            var task = _TestRequestHandler.SendRequest<UpdateEBookCommand, Result<Empty>>(updateEBookCommand);

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
            book.Product.Title = $"book-{_Randomizer.Int(1, 100)}";
            await _TestRepository.Add<E.EBook, Guid>(book);
            updateEBookCommand.Product_Title = book.Product.Title;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Title_Length_LessThan_3_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Product_Title = "ab";

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Title_Length_GreaterThan_50_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Product_Title = _Randomizer.String2(51);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_ImageFile_Size_GreaterThan_3Mb_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_ImageFile_Extension_NotImage_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Product_ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorId_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.AuthorIds = null;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.AuthorIds = [];

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Edition_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Edition = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Edition));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_NumberOfPages_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.NumberOfPages = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.NumberOfPages));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_DescriptionHtml_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Product_DescriptionHtml = null;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_DescriptionHtml));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_DescriptionHtml_Length_GreaterThan_500_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Product_DescriptionHtml = _Randomizer.String2(505);

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_DescriptionHtml));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_NumberOfInventory_LessThan_0_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Product_NumberOfInventory = -1;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_NumberOfInventory));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_Price_LessThanOrEqual_0_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Product_Price = 0;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_Price));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_TranslatorId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.TranslatorId = Guid.Empty;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.TranslatorId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_TranslatorId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.TranslatorId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.TranslatorId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_CategoryId_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Product_CategoryId = Guid.Empty;

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_CategoryId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Product_CategoryId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.Product_CategoryId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.Product_CategoryId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_PublisherId_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.PublisherId = Guid.NewGuid();

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.PublisherId));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_AuthorIds_NotExist_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.AuthorIds = [updateEBookCommand.AuthorIds[0], Guid.NewGuid()];

            //Act
            await requestAndGetResult();

            //
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.AuthorIds));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_EBookFile_Extension_NotAllowed_ShouldReturn_ValidationError()
        {
            //Arrange
            updateEBookCommand.EBookFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == nameof(updateEBookCommand.EBookFile));
            _OutPutValidationErrors(result);

        }































    }
}
