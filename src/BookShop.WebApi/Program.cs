global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using BookShop.Domain.Enums;
global using BookShop.Domain.QueryOptions;
global using BookShop.Application.Common.Dtos;
global using BookShop.Domain.Common.QueryOption;
using BookShop.Application;
using BookShop.Infrastructure.Setting;
using BookShop.Infrastructure;
using BookShop.Infrstructure.Persistance.SeedDatas;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookShop.WebApi.Services;


namespace BookShop.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices();

            InfrastructureSetting infrastructureSetting = builder.Configuration.GetSection(nameof(InfrastructureSetting)).Get<InfrastructureSetting>()!;
            builder.Services.AddInfrastructureServices(infrastructureSetting);
            builder.Services.AddScoped<JwtService>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // JWT Config
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false, // Set to true if you want to restrict to known clients
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseAuthorization();

            app.MapControllers();

            Seed seed = new Seed(app.Services);
            seed.MigrateDatabaseIfNotExist().GetAwaiter().GetResult();
            seed.SeedDatas().GetAwaiter().GetResult();

            app.Run();
        }
    }
}
