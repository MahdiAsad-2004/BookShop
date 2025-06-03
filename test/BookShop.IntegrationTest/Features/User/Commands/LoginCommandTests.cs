

using BookShop.Application.Features.User.Commands.Login;

namespace BookShop.IntegrationTest.Features.User.Commands
{
    public class LoginCommandTests : TestFeatureBase
    {
        LoginCommand loginCommand = new LoginCommand()
        {
            Username = $"username-{_Randomizer.Int(1, 100)}",
            Password = $"password-{_Randomizer.Int(1, 100)}",
        };
        private Result<LoginCommandResponse> result = new Result<LoginCommandResponse>();
        private readonly string userPassword = $"password-{_Randomizer.Byte(1, 100)}";
        private readonly E.Role role = new E.Role
        {
            Id = Guid.NewGuid(),
            Name = $"role-name-{_Randomizer.Byte(10 , 100)}",
            NormalizedName = string.Empty,
            ConcurrencyStamp = string.Empty,
        };
        private readonly E.User user = new E.User
        {
            Id = Guid.NewGuid(),
            Name = "test-name",
            Username = $"username-{_Randomizer.Byte(1, 100)}",
            PhoneNumber = string.Empty,
            Email = string.Empty,
            ConcurrencyStamp = string.Empty,
            ImageName = string.Empty,
            NormalizedEmail = string.Empty,
            NormalizedUsername = string.Empty,
            RoleId = null,
        };
        public LoginCommandTests(WebAppFactoryFixture webAppFactoryFixture, ITestOutputHelper testOutputHelper) : base(webAppFactoryFixture, testOutputHelper)
        {
        }
        private async Task requestAndGetResult()
        {
            result = await _TestRequestHandler.SendRequest<LoginCommand, Result<LoginCommandResponse>>(loginCommand);
        }
        private async Task addRequiredEntitrs()
        {
            user.PasswordHash = HashPassword(userPassword);
            if (user.RoleId != null)
                await _TestRepository.Add<E.Role, Guid>(role);
            await _TestRepository.Add<E.User, Guid>(user);
        }
        public override void Dispose()
        {
            _OutPutValidationErrors(result);
        }




        [Fact]
        public async Task ValidRequest_ShouldReturn_SuccessResult()
        {
            //Arrange
            loginCommand.Username = user.Username;
            loginCommand.Password = userPassword;
            await addRequiredEntitrs();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(result.Data.Id, user.Id);
            Assert.Equal(result.Data.Username, user.Username);
            Assert.Null(result.Data.Role);
        }


        [Fact]
        public async Task ValidRequest_With_Role_ShouldReturn_SuccessResult()
        {
            //Arrange
            loginCommand.Username = user.Username;
            loginCommand.Password = userPassword;
            user.RoleId = role.Id;
            await addRequiredEntitrs();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(result.Data.Id, user.Id);
            Assert.Equal(result.Data.Username, user.Username);
            Assert.Equal(result.Data.Role, role.Name);
        }


        [Fact]
        public async Task When_Username_IsWrong_ShouldReturn_AuthenticationError()
        {
            //Arrange
            loginCommand.Username = "asdfg";
            loginCommand.Password = userPassword;
            await addRequiredEntitrs();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorCode.Insufficient_Permissions, result.Error.Code);
        }


        [Fact]
        public async Task When_Password_IsWrong_ShouldReturn_AuthenticationError()
        {
            //Arrange
            loginCommand.Username = user.Username;
            loginCommand.Password = "asdfgh";
            await addRequiredEntitrs();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorCode.Insufficient_Permissions, result.Error.Code);
        }


        [Fact]
        public async Task When_Password_And_Username_AreWrong_ShouldReturn_AuthenticationError()
        {
            //Arrange
            loginCommand.Username = "dadadad";
            loginCommand.Password = "asdfgh";
            await addRequiredEntitrs();

            //Act
            await requestAndGetResult();

            //Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorCode.Insufficient_Permissions, result.Error.Code);
        }


        [Fact]
        public async Task When_Username_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            loginCommand.Username = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(loginCommand.Username));
        }


        [Fact]
        public async Task When_Username_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            loginCommand.Username = string.Empty;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(loginCommand.Username));
        }


        [Fact]
        public async Task When_Password_IsNull_ShouldReturn_ValidationError()
        {
            //Arrange
            loginCommand.Password = null;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(loginCommand.Password));
        }


        [Fact]
        public async Task When_Password_IsEmpty_ShouldReturn_ValidationError()
        {
            //Arrange
            loginCommand.Password = string.Empty;

            //Act
            await requestAndGetResult();

            //Assert
            _Assert_Result_Should_Be_ValidationError(result);
            _Assert_ValidationError_Conatain(result, nameof(loginCommand.Password));
        }





    }
}
