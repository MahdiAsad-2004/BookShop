﻿using BookShop.Domain.Identity;
using BookShop.Infrastructure.Persistance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BookShop.IntegrationTest.Common
{

    public class TestWebApplicationFactory : WebApplicationFactory<TestProgram>
    {
        private readonly string _connectionString;
        public TestWebApplicationFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<DbContextOptions<BookShopDbContext>>();

                services.RemoveAll<ICurrentUser>();

                services.AddTransient<ICurrentUser, TestCurrentUser>();

                services.AddDbContext<BookShopDbContext>(config =>
                {
                    config.UseSqlServer(_connectionString, sql =>
                    {
                        sql.CommandTimeout(6000);

                    });
                    config.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

                //services.AddScoped<TestRepositorye>();
                services.AddSingleton<TestProvider>();

            });


        }






    }
}
