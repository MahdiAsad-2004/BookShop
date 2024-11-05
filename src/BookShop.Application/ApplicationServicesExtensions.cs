using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Serilog;
using Serilog.Events;
using BookShop.Domain.Common.Event;
using BookShop.Application.Common.Event;
using BookShop.Application.Common.Mapper.Attribute;
using System.Diagnostics;
using BookShop.Application.Behaviours;
using BookShop.Application.Behaviours.PreProcessors;
using MediatR.Pipeline;

namespace BookShop.Application
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            services.AddScoped<IDomainEventPublisher, DomianEventPublisher>();
            
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddRequestPreProcessor(typeof(IRequestPreProcessor<>),typeof(LoggingPreProcessor<>));
                config.AddRequestPreProcessor(typeof(IRequestPreProcessor<>),typeof(ValidationPreProcessor<>));
                config.AddOpenBehavior(typeof(CacheBahviour<,>));
                config.AddOpenBehavior(typeof(AuthorizationBahviour<,>));
            });

            services.AddAutoMapper(config => 
            { 
                config.AddMaps(Assembly.GetExecutingAssembly());
            });
            
            services.AddSerilog(config =>
            {
                config.WriteTo.Console(LogEventLevel.Information);
            });

            AddMappers(services);


            return services;
        }







        private static void AddMappers(IServiceCollection services)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if(type.Name.EndsWith("Mapper") && type.GetCustomAttribute<InjectableMapperAttribute>() != null)
                {
                    services.AddScoped(type);
                }
            }            
        }











    }
}
