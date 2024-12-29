using System.Diagnostics.CodeAnalysis;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Data;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Gamestore;

[ExcludeFromCodeCoverage]
public static class ServicesRegistration
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Formatting = Formatting.Indented;
            options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            };

            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        builder.Services.AddDbContext<GameContext>(options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);

        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IGenreRepository, GenreRepository>();
        builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

        builder.Services.AddScoped<IGameControlService, GameControlService>();
        builder.Services.AddScoped<IGenreControlService, GenreControlService>();
        builder.Services.AddScoped<IPlatformControlService, PlatformControlService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .WithExposedHeaders("x-total-numbers-of-games");
            });
        });

        builder.Services.AddMemoryCache();
    }
}
