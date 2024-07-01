using System.IdentityModel.Tokens.Jwt;
using AllRiskSolutions_Desafio.Domain.ExternalServices;
using AllRiskSolutions_Desafio.Domain.Repositories;
using AllRiskSolutions_Desafio.Domain.Services;
using AllRiskSolutions_Desafio.Infrastructure.Repositories;
using AllRiskSolutions_Desafio.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace AllRiskSolutions_Desafio.Configuration;

public static class StartupExtension
{
    public static void AddStartupComponents(this IServiceCollection services)
    {
        services.AddVariableProvider();
        services.AddLogging();
        services.AddCacheManager();
        services.AddRepositories();
        services.AddServices();
        services.AddAuthenticationService();
        services
            .AddTransient<IAuthorizationMiddlewareResultHandler,
                AuthorizationMiddlewareResultHandler>();
        services.AddAuthorizationService();
        services.AddCors();

        services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        });
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddScoped<IUserRepository, UserRepositorySql>();
        services.AddScoped<ICityRepository, CityRepositorySql>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEncryptor, EncryptorBcrypt>();
        services.AddScoped<ICacheManager, CacheManagerRedis>();
        services.AddScoped<JwtSecurityTokenHandler>();
        services.AddScoped<UserService>();
        services.AddScoped<AuthService>();
        services.AddScoped<CityService>();
    }

    private static void AddVariableProvider(this IServiceCollection services)
    {
        services.AddSingleton<VariableProvider>();
    }

    private static void AddCacheManager(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = services.BuildServiceProvider()
                .GetRequiredService<VariableProvider>().GetCacheConnectionString();
            options.InstanceName = "AllRiskSolutions_Desafio";
        });
    }
}