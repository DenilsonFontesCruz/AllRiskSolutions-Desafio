namespace AllRiskSolutions_Desafio.Configuration;

public class Startup(WebApplicationBuilder builder)
{
    public void Start()
    {
        var services = builder.Services;

        var variableProvider = services.AddVariableProvider();
        services.AddLogging();
        services.AddCacheManager();
        services.AddExternalServices();
        services.AddRepositories();
        services.AddServices();
        services.AddCors();
        services.AddEndpointsApiExplorer();
        services.AddMiddlewares();
        services.AddSwaggerGen();

        var securityConfig = new SecurityConfig(services, variableProvider);
        securityConfig.AddAuthenticationService();
        securityConfig.AddAuthorizationService();
    }
}