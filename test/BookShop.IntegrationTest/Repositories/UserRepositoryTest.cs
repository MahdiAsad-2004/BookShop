
using BookShop.Domain.IRepositories;
using BookShop.IntegrationTest.Repositories.Base;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.IntegrationTest.Repositories
{
    public class UserRepositoryTest : TestRepositoriesBase
    {
        private IUserRepository _userRepository;
        public UserRepositoryTest(WebAppFactoryFixture webAppFactoryFixture, ITestOutputHelper testOutputHelper) : base(webAppFactoryFixture, testOutputHelper)
        {

        }
       

        public async Task GetUserTokeRequirements_When_Id_Exist_ShouldBe_Success()
        {
            //Arrange
            E.User user = new E.User
            {
                Id = Guid.NewGuid(),
                Name = "test-name",
                Username = "username-{_Randomizer.Byte(1, 100)}",
                PhoneNumber = string.Empty,
                Email = string.Empty,
                Role = new E.Role
                {
                    Id = Guid.NewGuid(),
                    Name = "role-name-test",
                    NormalizedName = string.Empty,
                },
                PasswordHash = "sdfdsfsdfsfsfd",
            };
            await _TestRepository.Add<E.User , Guid>(user);

            //Act
            using var scope = _ServiceScopeFactory.CreateScope();
            _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var result = await _userRepository.GetUserTokenRequirements(user.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Value.id);
            Assert.Equal(user.Username, result.Value.username);
            Assert.Equal(user.Role.Name, result.Value.role);
        }

        
        public async Task GetUserTokeRequirements_When_Id_NotExist_ShouldBe_Null()
        {
            //Arrange
            

            //Act
            using var scope = _ServiceScopeFactory.CreateScope();
            _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var result = await _userRepository.GetUserTokenRequirements(Guid.NewGuid());

            //Assert
            Assert.Null(result);
        }





    }
}
