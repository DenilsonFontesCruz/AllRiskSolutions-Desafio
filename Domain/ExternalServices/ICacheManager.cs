namespace AllRiskSolutions_Desafio.Domain.ExternalServices;

public interface ICacheManager
{
    Task SetAsync(string key, string value, TimeSpan? timeToLive = null);

    Task<string> GetAsync(string key);

    Task<bool> ExistsAsync(string key);
}