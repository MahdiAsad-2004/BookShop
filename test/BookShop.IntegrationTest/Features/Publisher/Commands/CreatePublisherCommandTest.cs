using BookShop.Application.Features.Publisher.Commands.Create;

namespace BookShop.IntegrationTest.Features.Publisher.Commands
{
    public class CreatePublisherCommandTest : TestFeatureBase
    {
        private Result<Empty> result = new Result<Empty>();
        CreatePublisherCommand createPublisherCommand = new CreatePublisherCommand
        {
            Title = "publisher-test",
            ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "publisher.png")),
        };
        public CreatePublisherCommandTest(WebAppFactoryFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
            : base(applicationCollectionFixture, testOutputHelper)
        {
             
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<CreatePublisherCommand, Result<Empty>>(createPublisherCommand);
        }




        [Fact]
        public async Task Valid_Request_ShouldReturn_SuccessResult_And_AddEntity_SaveImageFile()
        {
            //Arrnage 
            await _TestRepository.AddPermissionForUser(PermissionConstants.Publisher.Add);
            int fileCounts = Directory.GetFiles(PathExtensions.Publisher.Images).Count();
            int publishersCount = await _TestRepository.Count<E.Publisher, Guid>();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            int actualPublishersCount = await _TestRepository.Count<E.Publisher, Guid>();
            Assert.Equal(publishersCount + 1, actualPublishersCount);
            int actualFileCount = Directory.GetFiles(PathExtensions.Publisher.Images).Count();
            Assert.Equal(fileCounts + 1, actualFileCount);
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
            createPublisherCommand.Title = publisher.Title;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createPublisherCommand.Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task Without_RequiredPermissions_ShouldThrow_UnauthorizeException()
        {
            //Arrange


            //Act
            var task = _TestRequestHandler.SendRequest<CreatePublisherCommand, Result<Empty>>(createPublisherCommand);

            //Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () =>
            {
                await task;
            });
        }


        [Fact]
        public async Task When_ImageFile_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createPublisherCommand.ImageFile = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createPublisherCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_ImageFile_Size_GreaterThan_3MB_ShouldReturn_ValidationError()
        {
            //Arrange
            createPublisherCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "3Mb.jpg"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createPublisherCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_ImageFile_Extension_NotImage_ShouldReturn_ValidationError()
        {
            //Arrange
            createPublisherCommand.ImageFile = FileExtensions.CreateIFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Files", "text.txt"));

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createPublisherCommand.ImageFile));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Title_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            createPublisherCommand.Title = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createPublisherCommand.Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Title_Length_LessThan_3_ShouldReturn_ValidationError()
        {
            //Arrange
            createPublisherCommand.Title = "ab";

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createPublisherCommand.Title));
            _OutPutValidationErrors(result);
        }


        [Fact]
        public async Task When_Title_Length_GreaterThan_30_ShouldReturn_ValidationError()
        {
            //Arrange
            createPublisherCommand.Title = _Randomizer.String2(31);

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(createPublisherCommand.Title));
            _OutPutValidationErrors(result);
        }




    }
}
