using Microsoft.Extensions.Configuration;
using Respawn;

namespace BookShop.IntegrationTest.Common
{
    [Collection("WebAppFixture")]

    public class TestBase : IDisposable
    {
        public readonly WebAppFixture _webAppFixture;
        public TestBase(WebAppFixture webAppFixture)
        {
            _webAppFixture = webAppFixture;
            BeforeEachTest().GetAwaiter().GetResult();
        }


        private async Task AfterEachTest()
        {
        }

        private async Task BeforeEachTest()
        {
            await _webAppFixture._respawner.ResetAsync(ConnectionString);
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

    }
}
