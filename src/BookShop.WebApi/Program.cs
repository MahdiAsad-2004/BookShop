using BookShop.Application;
using BookShop.Infrastructure;
using BookShop.Infrastructure.Setting;
using BookShop.Infrstructure.Persistance.SeedDatas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();

InfrastructureSetting infrastructureSetting = builder.Configuration.GetSection(nameof(InfrastructureSetting)).Get<InfrastructureSetting>()!;

builder.Services.AddInfrastructureServices(infrastructureSetting);

builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

Seed seed = new Seed(app.Services);
await seed.SeedDatas();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }
