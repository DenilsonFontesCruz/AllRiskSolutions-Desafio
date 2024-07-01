using AllRiskSolutions_Desafio.Configuration;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Domain.Repositories;
using AllRiskSolutions_Desafio.Utils;
using Microsoft.EntityFrameworkCore;

namespace AllRiskSolutions_Desafio.Infrastructure.Repositories;

public class CityRepositorySql(AppDbContext context, ILogger<CityRepositorySql> logger) : ICityRepository
{
    public async Task<City?> InsertAsync(City model)
    {
        var result = await context.Cities.AddAsync(model);
        return result.Entity;
    }

    public async Task InsertManyAsync(IEnumerable<City> models)
    {
        await context.Cities.AddRangeAsync(models);
    }

    public async Task<Result<City>> FindByIdAsync(Guid id)
    {
        var city = await context.Cities.FirstOrDefaultAsync(x => x.Id == id);
        if (city == null)
        {
            logger.LogInformation($"City not found, Id: {id.ToString()}");
            return Result.Fail<City>("City not found", "404");
        }

        return city;
    }

    public async Task<Result<IEnumerable<City>>> FindAllAsync()
    {
        var cities = await context.Cities.ToListAsync();
        return cities;
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await context.Cities.AnyAsync(x => x.Name == name);
    }

    public async Task<Result<City>> FindByNameAsync(string name)
    {
        var city = await context.Cities.FirstOrDefaultAsync(x => x.Name == name);
        if (city == null)
        {
            logger.LogInformation($"City not found, Name: {name}");
            return Result.Fail<City>("City not found", "404");
        }

        return city;
    }

    public async Task<int> SaveAsync()
    {
        return await context.SaveChangesAsync();
    }
}