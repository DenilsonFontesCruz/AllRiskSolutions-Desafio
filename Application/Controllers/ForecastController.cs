using AllRiskSolutions_Desafio.Domain.ApiServices;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Domain.Services;
using AllRiskSolutions_Desafio.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllRiskSolutions_Desafio.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class ForecastController(
    ForecastService forecastService,
    CityService cityService,
    ILogger<ForecastController> logger)
    : ControllerBase
{
    [HttpGet("current")]
    [AllowAnonymous]
    public async Task<ActionResult<WeatherInfo>> GetCurrentWeather([FromQuery] Coords? coords,
        string? cityId)
    {
        try
        {
            var cityCoordsResult = await GetCityCoords(coords, cityId);

            if (cityCoordsResult.IsFail)
            {
                return Result.Fail<WeatherInfo>(cityCoordsResult.Error);
            }

            var resultWeather = await forecastService.GetCurrentWheater(cityCoordsResult.Value());

            return resultWeather;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting current weather");
            throw;
        }
    }

    [HttpGet("forecast-five-days")]
    [AllowAnonymous]
    public async Task<ActionResult<List<SimpleWeatherInfo>>> GetFiveDaysForecast(
        [FromQuery] Coords? coords,
        string? cityId)
    {
        try
        {
            var cityCoordsResult = await GetCityCoords(coords, cityId);

            if (cityCoordsResult.IsFail)
            {
                return Result.Fail<List<SimpleWeatherInfo>>(cityCoordsResult.Error);
            }

            var resultWeather = await forecastService.GetFiveDaysForecast(cityCoordsResult.Value());

            return resultWeather;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting five days forecast");
            throw;
        }
    }

    private async Task<Result<Coords>> GetCityCoords(Coords? coords, string? cityId)
    {
        Guid.TryParse(cityId, out var id);
        var cityResult = (id != Guid.Empty)
            ? await cityService.GetById(id)
            : Result.Fail<City>("City id must be provided", "400");

        if (cityResult.IsFail && coords!.IsEmpty())
        {
            return Result.Fail<Coords>("City coords or id must be provided", "400");
        }

        return cityResult.IsSuccess
            ? cityResult.Value().GetCoords()
            : coords!;
    }
}