global using Bogus;
global using Xunit.Abstractions;
global using BookShop.Domain.Enums;
global using BookShop.Domain.Common;
global using BookShop.Domain.Constants;
global using BookShop.Domain.Exceptions;
global using BookShop.IntegrationTest.Common;
global using BookShop.Application.Extensions;
global using BookShop.IntegrationTest.Fixture;
global using BookShop.IntegrationTest.Features.Base;
using Microsoft.Extensions.DependencyInjection;
using BookShop.Domain.Identity;

namespace BookShop.IntegrationTest.Features.Base
{
    public abstract class TestFeatureBase : WebAppFactoryTestBase, IDisposable
    {
        protected readonly TestCache _TestCache;
        protected ITestOutputHelper _TestOutputHelper;
        protected readonly TestRepository _TestRepository;
        protected readonly TestRequestHandler _TestRequestHandler;
        protected readonly TestCurrentUserSetting _TestCurrentUserSetting;
        protected static readonly Randomizer _Randomizer = new Randomizer();
        private readonly IServiceScopeFactory _serviceScopeFactory;

        // Before Each Test
        public TestFeatureBase(WebAppFactoryFixture webAppFactoryFixture, ITestOutputHelper testOutputHelper)
        {
            _TestOutputHelper = testOutputHelper;
            _TestCache = webAppFactoryFixture.TestCache;
            _TestRepository = webAppFactoryFixture.TestRepository;
            _TestRequestHandler = webAppFactoryFixture.TestRequestHandler;
            _TestCurrentUserSetting = new TestCurrentUserSetting(webAppFactoryFixture.ServiceScopeFactory);
            webAppFactoryFixture.Respawner.ResetAsync(ConnectionStringProvider.Get()).GetAwaiter().GetResult();
            _TestRepository.ClearUsersExceptTestUser().GetAwaiter().GetResult();
            _TestCache.RemoveUserPermissionsCache();
            _TestCurrentUserSetting.SetCurrentUser();
            _serviceScopeFactory = webAppFactoryFixture.ServiceScopeFactory;
        }

        // After Each Test
        public virtual void Dispose()
        {

        }


        protected void _Assert_Result_Should_Be_ValidationError<TData>(Result<TData> result)
        {
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorCode.Validation, result.Error.Code);
        }


        protected void _Assert_ValidationError_Conatain<TDate>(Result<TDate> result, string propertyName)
        {
            Assert.Contains(result!.Error!.ValidationErrors, a => a.PropertyName == propertyName);
        }



        protected void _OutPutValidationErrors<TData>(Result<TData> result)
        {
            if (result != null && result.Error != null && result.Error.ValidationErrors != null)
            {
                for (int i = 0; i < result.Error.ValidationErrors.Count; i++)
                {
                    _TestOutputHelper.WriteLine($"Validation Error {i + 1}: {result.Error.ValidationErrors[i].ErrorMessage}");
                }
            }
        }



        public string HashPassword(string password)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IPasswordHasher passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
                return passwordHasher.Hash(password);
            }
        }



        public static string PhysicalPath(string path)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), path);
        }



    }
}
