using System.IdentityModel.Tokens.Jwt;
using AllRiskSolutions_Desafio.Application.Api;
using AllRiskSolutions_Desafio.Domain.ApiServices;
using AllRiskSolutions_Desafio.Domain.ExternalServices;
using AllRiskSolutions_Desafio.Domain.Repositories;
using AllRiskSolutions_Desafio.Domain.Services;
using AllRiskSolutions_Desafio.Infrastructure.Repositories;
using AllRiskSolutions_Desafio.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;

namespace AllRiskSolutions_Desafio.Configuration;

public static class StartupExtension
{
    public static void AddStartup(this WebApplicationBuilder builder)
    {
        new Startup(builder).Start();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddScoped<IUserRepository, UserRepositorySql>();
        services.AddScoped<ICityRepository, CityRepositorySql>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<AuthService>();
        services.AddScoped<CityService>();
        services.AddScoped<ForecastService>();
    }

    public static void AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped<HttpClient>();
        services.AddScoped<JwtSecurityTokenHandler>();
        services.AddScoped<IEncryptor, EncryptorBcrypt>();
        services.AddScoped<ICacheManager, CacheManagerRedis>();
        services.AddScoped<IWeatherApi, WeatherApi>();
    }

    public static VariableProvider AddVariableProvider(this IServiceCollection services)
    {
        services.AddSingleton<VariableProvider>();
        return services.BuildServiceProvider().GetService<VariableProvider>()!;
    }

    public static void AddCacheManager(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = services.BuildServiceProvider()
                .GetRequiredService<VariableProvider>().GetCacheConnectionString();
            options.InstanceName = "AllRiskSolutions_Desafio";
        });
    }

    public static void AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<AuthorizationMiddlewareResultHandler>();
    }

    public static void AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo()
                    { Title = "AllRiskSolutions-Desafio-Weather-Api", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
    }
}