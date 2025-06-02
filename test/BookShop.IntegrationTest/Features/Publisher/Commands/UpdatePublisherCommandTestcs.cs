using BookShop.Application.Features.Publisher.Commands.Update;

namespace BookShop.IntegrationTest.Features.Publisher.Commands
{
    public class UpdatePublisherCommandTest : TestFeatureBase
    {
        private readonly E.Publisher _savedPublisher = new E.Publisher()
        {
            Id = Guid.NewGuid(),
            CreateBy = string.Empty,
            CreateDate = DateTime.UtcNow,
            LastModifiedBy = string.Empty,
            LastModifiedDate = DateTime.UtcNow,
            Title = "test-publisher"
        };
        private UpdatePublisherCommand updatePublisherCommand = new UpdatePublisherCommand
        {
            Title = _Randomizer.String2(10),
        };
        private Result<Empty> result = new Result<Empty>();
        public UpdatePublisherCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
            updatePublisherCommand.Id = _savedPublisher.Id;
            addPublisher().GetAwaiter().GetResult();
             
        }
        private async Task addPublisher()
        {
            await _TestRepository.Add<E.Publisher, Guid>(_savedPublisher);
        }
        private void assert_Publisher_Updated(E.Publisher? publisher)
        {
            Assert.NotNull(publisher);
            Assert.Equal(updatePublisherCommand.Title, publisher.Title);
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<UpdatePublisherCommand, Result<Empty>>(updatePublisherCommand);
        }





        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_UpdateEntity()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.Publisher.Update);

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.Publisher, Guid>(updatePublisherCommand.Id);
            assert_Publisher_Updated(entity);
        }


        [Fact]
        public async Task Valid_Request_With_ImageFile_ShouldReturn_SuccessResult_And_UpdateEntity_SaveImageFile()
        {
            //Arrnage 
            updatePublisherCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "publisher.png"));
            await _TestRepository.AddPermissionForUser(PermissionConstants.Publisher.Update);
            int imagesCount = Directory.GetFiles(PhysicalPath(PathExtensions.Publisher.Images)).Count();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var entity = await _TestRepository.Get<E.Publisher, Guid>(updatePublisherCommand.Id);
            assert_Publisher_Updated(entity);
            int actualImagesCount = Directory.GetFiles(PhysicalPath(PathExtensions.Publisher.Images)).Count();
            Assert.Equal(imagesCount + 1, actualImagesCount);
        }


        [Fact]
        public async Task When_Title_IsDuplicate_ShouldReturn_ValidationError()
        {
            //Arrnage 
            E.Publisher publisher = new E.Publisher
            {
                Id = Guid.NewGuid(),
                CreateBy = string.Empty,
                CreateDate = DateTime.UtcNow,
                Title = $"publisher-{Random.Shared.Next(1, 100)}",
            };
            await _TestRepository.Add<E.Publisher, Guid>(publisher);
            updatePublisherCommand.Title = publisher.Title;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updatePublisherCommand.Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnauthorizeException()
        {
            //Arrange


            //Act
            var task = _TestRequestHandler.SendRequest<UpdatePublisherCommand, Result<Empty>>(updatePublisherCommand);

            //Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () =>
            {
                await task;
            });
        }


        [Fact]
        public async Task When_ImageFile_Size_GreaterThan_3MB_ShouldReturn_ValidationError()
        {
            //Arrange
            updatePublisherCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updatePublisherCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_ImageFile_Extension_NotImage_ShouldReturn_ValidationError()
        {
            //Arrange
            updatePublisherCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updatePublisherCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Title_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            updatePublisherCommand.Title = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updatePublisherCommand.Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Title_Length_LessThan_3_ShouldReturn_ValidationError()
        {
            //Arrange
            updatePublisherCommand.Title = "ab";

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updatePublisherCommand.Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Title_Length_GreaterThan_30_ShouldReturn_ValidationError()
        {
            //Arrange
            updatePublisherCommand.Title = _Randomizer.String2(31);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(updatePublisherCommand.Title));
            _OutPutValidationErrors(result);
        }










    }
}
