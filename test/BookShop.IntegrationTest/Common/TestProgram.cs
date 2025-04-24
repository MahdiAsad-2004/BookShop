using BookShop.Application;
using BookShop.Infrastructure;
using BookShop.Infrastructure.Setting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = "BookShop.IntegrationTest",
    EnvironmentName = "Development",
    ContentRootPath = "C:\\Users\\Mahdi\\source\\repos\\My-Projects\\BookShop\\test\\BookShop.IntegrationTest",
});

builder.Services.AddApplicationServices();

InfrastructureSetting infrastructureSetting = builder.Configuration.GetSection(nameof(InfrastructureSetting)).Get<InfrastructureSetting>()!;

infrastructureSetting = new InfrastructureSetting
{
    PersistanceSetting = new PersistanceSetting
    {
        ConnectionString = "",
        DataBase = "SqlServer",
    }
};

builder.Services.AddInfrastructureServices(infrastructureSetting);

var app = builder.Build();

app.Run();


public partial class TestProgram { }
