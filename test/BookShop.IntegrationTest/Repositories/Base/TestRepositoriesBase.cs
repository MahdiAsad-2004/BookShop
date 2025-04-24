
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.IntegrationTest.Repositories.Base
{
    public class TestRepositoriesBase : WebAppFactoryTestBase, IDisposable
    {
        protected ITestOutputHelper _TestOutputHelper;
        protected readonly TestRepository _TestRepository;
        protected readonly IServiceScopeFactory _ServiceScopeFactory;
        protected static readonly Randomizer _Randomizer = new Randomizer();

        // Before Each Test
        public TestRepositoriesBase(WebAppFactoryFixture webAppFactoryFixture, ITestOutputHelper testOutputHelper)
        {
            _TestOutputHelper = testOutputHelper;
            _TestRepository = webAppFactoryFixture.TestRepository;
            _ServiceScopeFactory = webAppFactoryFixture.ServiceScopeFactory;
            webAppFactoryFixture.Respawner.ResetAsync(ConnectionStringProvider.Get()).GetAwaiter().GetResult();
            _TestRepository.ClearUsersExceptTestUser().GetAwaiter().GetResult();    
        }

        // After Each Test
        public virtual void Dispose()
        {

        }



    }
}
