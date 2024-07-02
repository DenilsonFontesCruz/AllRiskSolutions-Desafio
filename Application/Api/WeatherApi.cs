using AllRiskSolutions_Desafio.Configuration;
using AllRiskSolutions_Desafio.Domain.ApiServices;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Utils;

namespace AllRiskSolutions_Desafio.Application.Api;

public class WeatherApi(
    VariableProvider variableProvider,
    HttpClient httpClient,
    ILogger<WeatherApi> logger) : IWeatherApi
{
    public async Task<Result<WeatherInfo>> GetCurrentWheater(Coords coords)
    {
        var result = await httpClient.GetAsync(
            $"https://api.openweathermap.org/data/2.5/weather?lat={coords.Latitude}" +
            $"&lon={coords.Longitude}&appid={variableProvider.GetWeatherApiKey()}");

        if (!result.IsSuccessStatusCode)
        {
            logger.LogError("Error fetching data from OpenWeatherMap");
            Result.Fail<WeatherInfo>("Error fetching data from OpenWeatherMap", "500");
        }

        var content = await result.Content.ReadFromJsonAsync<GetCurrentWheaterApiResponse>();

        if (content == null)
        {
            logger.LogError("Error parsing data from OpenWeatherMap");
            Result.Fail<WeatherInfo>("Error parsing data from OpenWeatherMap", "500");
        }

        return Result.Success<WeatherInfo>(content!);
    }

    public async Task<Result<List<SimpleWeatherInfo>>> GetFiveDaysForecast(Coords coords)
    {
        var result = await httpClient.GetAsync(
            $"https://api.openweathermap.org/data/2.5/forecast?lat={coords.Latitude}" +
            $"&lon={coords.Longitude}&appid={variableProvider.GetWeatherApiKey()}");

        if (!result.IsSuccessStatusCode)
        {
            logger.LogError("Error fetching data from OpenWeatherMap");
            Result.Fail<WeatherInfo>("Error fetching data from OpenWeatherMap", "500");
        }

        var content = await result.Content.ReadFromJsonAsync<GetCurrentForecastApiResponse>();

        if (content == null)
        {
            logger.LogError("Error parsing data from OpenWeatherMap");
            Result.Fail<WeatherInfo>("Error parsing data from OpenWeatherMap", "500");
        }

        return Result.Success<List<SimpleWeatherInfo>>(content!);
    }
}

public record Weather(string Main);

public record Main(
    double Temp,
    double Feels_like,
    double Temp_min,
    double Temp_max,
    int Pressure,
    int Humidity);

public record GetCurrentWheaterApiResponse(
    List<Weather> Weather,
    Main Main,
    long Dt
)
{
    public static implicit operator WeatherInfo(GetCurrentWheaterApiResponse response)
    {
        var (weather, main, dt) = response;

        var dateTime = DateTimeOffset.FromUnixTimeSeconds(dt).DateTime;

        var climate = weather.First().Main switch
        {
            "Thunderstorm" => Climate.Thunderstorm,
            "Drizzle" => Climate.Drizzle,
            "Rain" => Climate.Rain,
            "Snow" => Climate.Snow,
            "Atmosphere" => Climate.Atmosphere,
            "Clear" => Climate.Clear,
            "Clouds" => Climate.Clouds,
            _ => Climate.Clear
        };

        return new WeatherInfo(dateTime, climate, main.Temp, main.Feels_like, main.Temp_max,
            main.Temp_min, main.Pressure, main.Humidity);
    }

    public static implicit operator SimpleWeatherInfo(GetCurrentWheaterApiResponse response)
    {
        var (weather, main, dt) = response;

        var dateTime = DateTimeOffset.FromUnixTimeSeconds(dt).DateTime;

        var climate = weather.First().Main switch
        {
            "Thunderstorm" => Climate.Thunderstorm,
            "Drizzle" => Climate.Drizzle,
            "Rain" => Climate.Rain,
            "Snow" => Climate.Snow,
            "Atmosphere" => Climate.Atmosphere,
            "Clear" => Climate.Clear,
            "Clouds" => Climate.Clouds,
            _ => Climate.Clear
        };

        return new SimpleWeatherInfo(dateTime, climate, main.Temp_max, main.Temp_min);
    }
}

public record GetCurrentForecastApiResponse(List<GetCurrentWheaterApiResponse> list)
{
    public static implicit operator List<SimpleWeatherInfo>(GetCurrentForecastApiResponse response)
    {
        return response.list.Select(r => (SimpleWeatherInfo)r).ToList();
    }
}