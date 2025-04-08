global using E = BookShop.Domain.Entities;
using Azure.Core;
using Azure;
using Bogus;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using BookShop.Application.Caching;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using BookShop.Application.Common.Request;
using Xunit.Abstractions;
using BookShop.Domain.Common;


namespace BookShop.IntegrationTest.Application.Common
{
    [Collection("ApplicationCollectionFixture")]
    public class TestBase : IDisposable
    {
        internal readonly TestDbContext _TestDbContext;
        public readonly ApplicationCollectionFixture _applicationCollectionFixture;
        internal ITestOutputHelper _testOutputHelper;
        public TestBase(ApplicationCollectionFixture applicationCollectionFixture, ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _applicationCollectionFixture = applicationCollectionFixture;
            _TestDbContext = new TestDbContext(_applicationCollectionFixture._serviceScopeFactory);
            BeforeEachTest().GetAwaiter().GetResult();
        }


        private async Task AfterEachTest()
        {
        }

        private async Task BeforeEachTest()
        {
            await _applicationCollectionFixture._respawner.ResetAsync(ConnectionString);
        }

        public static string ConnectionString
        {
            get { return GetConectionString(); }
        }

        private static string GetConectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                //.AddEnvironmentVariables()
                .Build();

            string path = "ConnectionStrings:TestDatabaseConnectionString";

            string? connectionString = configuration.GetValue<string>(path);

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception($"Connection string in path {path} not found");

            return connectionString;
        }

        public async void Dispose()
        {
            await AfterEachTest();
        }



      
        public async Task SendRequest<TRequest>(TRequest request) where TRequest : MediatR.IRequest
        {
            using (var scopr = _applicationCollectionFixture._serviceScopeFactory.CreateScope())
            {
                IMediator mediator = scopr.ServiceProvider.GetRequiredService<IMediator>(); 
                await mediator.Send(request);
            }
        }
        
        public async Task<TRessponse> SendRequest<TRequest,TRessponse>(TRequest request) where TRequest : IRequest<TRessponse>
        {
            using (var scopr = _applicationCollectionFixture._serviceScopeFactory.CreateScope())
            {
                IMediator mediator = scopr.ServiceProvider.GetRequiredService<IMediator>(); 
                TRessponse ressponse = await mediator.Send(request);
                return ressponse;
            }
        }



        public void SetCurrentUser()
        {
            using (var scope = _applicationCollectionFixture._serviceScopeFactory.CreateScope())
            {
                CurrentUserCacheObject currentUserCacheObject = new CurrentUserCacheObject
                {
                    Authenticated = true,
                    Email = TestCurrentUser.User.Email,
                    Id = TestCurrentUser.User.Id,
                    Name = TestCurrentUser.User.Name,
                    PhoneNumber = TestCurrentUser.User.PhoneNumber,
                    Username = TestCurrentUser.User.Username
                };
                IMemoryCache memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                memoryCache.Set(TestCurrentUser.CurrentUserCachKey, currentUserCacheObject, DateTimeOffset.UtcNow.AddMinutes(1));
            }
        }


        public async Task SetCurrentUserAnonymous()
        {
            using (var scope = _applicationCollectionFixture._serviceScopeFactory.CreateScope())
            {
                IMemoryCache memoryCache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                memoryCache.Remove(TestCurrentUser.CurrentUserCachKey);
            }
        }



        public async Task<TResponse?> GetFromCache<TRequest, TResponse>(TRequest request)
            where TRequest : CachableRequest<TResponse> 
            where TResponse : class
        {
            using (var scope = _applicationCollectionFixture._serviceScopeFactory.CreateScope())
            {
                var x = request.GetCacheKey();
                ICache cache = scope.ServiceProvider.GetRequiredService<ICache>();
                TResponse? response = cache.GetOrDefault<TResponse>(request.GetCacheKey());
                return response;
            }
        }


        protected void _Assert_Result_Should_Be_ValidationError<TData>(Result<TData> result)
        {
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorCode.Validation, result.Error.Code);
        }

        protected void _OutPutValidationErrors<TData>(Result<TData> result)
        {
            for (int i = 0; i < result!.Error!.ValidationErrors.Count; i++)
            {
                _testOutputHelper.WriteLine($"Validation Error {i+1}: {result.Error.ValidationErrors[i].ErrorMessage}");
            }
        }


    }
}
