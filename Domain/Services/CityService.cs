using System.Globalization;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Domain.Repositories;
using AllRiskSolutions_Desafio.Utils;

namespace AllRiskSolutions_Desafio.Domain.Services;

public class CityService(ICityRepository cityRepository, ILogger<CityService> logger)
{
    public async Task<Result<City>> Register(string name, string countryCode, Coords coords)
    {
        logger.LogInformation("Registering city");

        if (await cityRepository.ExistsByNameAsync(name))
        {
            logger.LogInformation($"City: {name} already exists");
            return Result.Fail<City>($"City: {name} already exists", "409");
        }

        var city = new City(Guid.NewGuid(), name, coords, countryCode);

        await cityRepository.InsertAsync(city);
        await cityRepository.SaveAsync();

        return city;
    }

    public async Task<Result<City>> GetByName(string name)
    {
        logger.LogInformation("Finding city by name");
        var cityResult = await cityRepository.FindByNameAsync(name);

        if (cityResult.IsFail)
        {
            return cityResult;
        }

        return cityResult.Value();
    }

    public async Task<bool> ExistsByName(string name)
    {
        return await cityRepository.ExistsByNameAsync(name);
    }
}