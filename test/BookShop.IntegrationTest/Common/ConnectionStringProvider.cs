
using Microsoft.Extensions.Configuration;

namespace BookShop.IntegrationTest.Common
{
    internal static class ConnectionStringProvider
    {

        private static string? _conn = null;

        public static string Get()
        {
            if (string.IsNullOrWhiteSpace(_conn) == false)
                return _conn;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                //.AddEnvironmentVariables()
                .Build();

            string path = "ConnectionStrings:TestDatabaseConnectionString";

            string? connectionString = configuration.GetValue<string>(path);

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception($"Connection string in path {path} not found");

            _conn = connectionString;

            return connectionString;
        }

    }


    public class TestProvider
    {

    }

}
