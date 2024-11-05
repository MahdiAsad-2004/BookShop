using BookShop.Application;
using BookShop.Infrastructure;
using BookShop.Infrastructure.Setting;

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

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
