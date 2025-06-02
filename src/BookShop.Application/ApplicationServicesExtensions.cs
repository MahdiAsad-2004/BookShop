global using E = BookShop.Domain.Entities;
global using MediatR;
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
using BookShop.Application.Common.Rules;
using BookShop.Application.Features.Book.Commands.Create;

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
                //config.AddRequestPreProcessor(typeof(IRequestPreProcessor<>),typeof(ValidationPreProcessor<>));
                config.AddOpenBehavior(typeof(ValidationBahviour<,>));
                config.AddOpenBehavior(typeof(AuthorizationBahviour<,>));
                config.AddOpenBehavior(typeof(CacheBahviour<,>));
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
            AddBussinessRules(services);

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

        private static void AddBussinessRules(IServiceCollection services)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.Name.EndsWith("Rule") && type.BaseType?.GetGenericTypeDefinition() == typeof(BussinessRule<>))
                {
                    services.AddScoped(type.BaseType , type);
                }
            }
        }











    }
}
