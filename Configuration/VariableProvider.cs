namespace AllRiskSolutions_Desafio.Configuration;


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

    public string GetWeatherApiKey()
    {
        return configuration["WeatherApiKey"] ?? "ApiKey";
    }

    public string GetSqlDatabaseConnectionString()
    {
        return configuration["SqlDatabaseConnectionString"] ?? "DataSource=app.db;Cache=Shared";
    }
}