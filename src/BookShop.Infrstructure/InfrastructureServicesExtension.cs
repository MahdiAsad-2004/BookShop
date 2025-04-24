using BookShop.Application.Serializition;
using BookShop.Application;
using BookShop.Infrastructure.Serializition;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BookShop.Domain.Common.Repository;
using BookShop.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using BookShop.Domain.Identity;
using BookShop.Application.Caching;
using BookShop.Infrastructure.Caching;
using BookShop.Application.Authorization;
using BookShop.Infrastructure.Authorization;
using BookShop.Infrastructure.Setting;

namespace BookShop.Infrastructure
{
    public static class InfrastructureServicesExtension
    {
        private static InfrastructureSetting _setting;
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, InfrastructureSetting infrastructureSetting)
        {
            _setting = infrastructureSetting;

            services.AddScoped<IJsonSerializer, Infrastructure.Serializition.JsonSerializer>();

            services.AddScoped<IPermissionChecker, PermissionChecker>();

            AddDatabase(services, _setting.PersistanceSetting.ConnectionString);

            AddRepositories(services);

            AddIdentity(services);

            AddCache(services);

            AddCurrentUser(services);

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }



        private static void AddIdentity(IServiceCollection services)
        {
            //services.AddIdentityCore<User>(options =>
            //{
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;
            //});
            //services.AddTransient<IUserStore<User>, Infrastructure.Identity.UserStore>();
            //services.AddTransient<IRoleStore<Role>, Infrastructure.Identity.RoleStore>();
            //services.AddScoped<Domain.Identity.IPasswordHasher, Infrastructure.Identity.PasswordHasher>();
            //services.Configure<DataProtectionTokenProviderOptions>(options =>
            //{
            //    options.TokenLifespan = TimeSpan.FromHours(3);
            //});
            ////services.Configure<PasswordHasherOptions>(options =>
            ////{
            ////    options.IterationCount = 10000;
            ////    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2;
            ////});

        }



        private static void AddRepositories(IServiceCollection services)
        {
            string projectName = nameof(BookShop);
            LoadAssemblies();
            var assemlies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith(projectName));
            var types = assemlies.SelectMany(a => a.GetTypes());
            var repositotiesImpls = types.Where(a => a.Name.EndsWith("Repository") && a.IsClass);
            foreach (var repositotiesImpl in repositotiesImpls)
            {
                Type? repositoryInterface = GetRepositoryInterface(repositotiesImpl);

                //Console.WriteLine(repositotiesImpl.Name);
                //Console.WriteLine(repositoryInterface.Name);
                //Console.WriteLine("---------------------------------------------");
                if (repositoryInterface != null)
                {
                    services.AddScoped(repositoryInterface, repositotiesImpl);
                }
            }
        }

        private static void LoadAssemblies()
        {
            string projectName = nameof(BookShop);
            AppDomain.CurrentDomain.Load($"{projectName}.{nameof(Domain)}");
            AppDomain.CurrentDomain.Load($"{projectName}.{nameof(Application)}");
        }


        private static Type? GetRepositoryInterface(Type repositoryImpl)
        {
            Type? respositoryInterface = repositoryImpl.GetInterfaces()
                .FirstOrDefault(a => a.IsAssignableTo(typeof(IRepository)) && a != typeof(IRepository));

            //if (respositoryInterface == null)
            //    throw new Exception($"Repository {repositoryImpl.Name} Interface not found!");

            return respositoryInterface;
        }



        private static void AddDatabase(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BookShopDbContext>(config =>
            {
                config.UseSqlServer(connectionString, sql =>
                {
                    sql.CommandTimeout(6000);
                });
                config.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }


        private static void AddCache(IServiceCollection services)
        {
            services.AddMemoryCache(options =>
            {

            });
            services.AddTransient<ICache, InMemoryCache>();
        }





        private static void AddCurrentUser(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentWebUser>();
        }



    }
}
