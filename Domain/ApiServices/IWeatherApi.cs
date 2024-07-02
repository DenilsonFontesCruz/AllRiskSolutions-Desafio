using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Utils;

namespace AllRiskSolutions_Desafio.Domain.ApiServices;

public interface IWeatherApi
{
    Task<Result<WeatherInfo>> GetCurrentWheater(Coords coords);

    Task<Result<List<WeatherInfo>>> GetFiveDaysForecast(Coords coords);
}

public record WeatherInfo(
    DateTime DateTime,
    Climate Climate,
    double Temperature,
    double FeelsLike,
    double MaxTemperature,
    double MinTemperature,
    double Pressure,
    double Humidity);

public enum Climate
{
    Thunderstorm = 1,
    Drizzle = 2,
    Rain = 3,
    Snow = 4,
    Atmosphere = 5,
    Clear = 6,
    Clouds = 7
}