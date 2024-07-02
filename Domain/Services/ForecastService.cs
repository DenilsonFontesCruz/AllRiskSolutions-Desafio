using AllRiskSolutions_Desafio.Domain.ApiServices;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Utils;

namespace AllRiskSolutions_Desafio.Domain.Services;

public class ForecastService(IWeatherApi weatherApi, ILogger<ForecastService> logger)
{
    public async Task<Result<WeatherInfo>> GetCurrentWheater(Coords coords)
    {
        logger.LogInformation("Getting current weather");
        return await weatherApi.GetCurrentWheater(coords);
    }

    public async Task<Result<List<SimpleWeatherInfo>>> GetFiveDaysForecast(Coords coords)
    {
        logger.LogInformation("Getting five days forecast");
        return await weatherApi.GetFiveDaysForecast(coords);
    }
}