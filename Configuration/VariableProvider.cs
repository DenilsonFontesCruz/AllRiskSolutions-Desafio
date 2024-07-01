namespace AllRiskSolutions_Desafio.Configuration;

/// <summary>
/// Get variables from appsettings.json
/// </summary>
/// <param name="configuration">IConfiguration</param>
public class VariableProvider(IConfiguration configuration)
{
    public string GetJwtSecret()
    {
        return configuration["JwtSecret"] ?? "SecretTokenForDevelopmentAndTesting";
    }

    public string GetCacheConnectionString()
    {
        return configuration["CacheConnectionString"] ?? "127.0.0.1:6379";
    }
}