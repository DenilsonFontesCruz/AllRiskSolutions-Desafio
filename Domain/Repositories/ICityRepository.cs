using AllRiskSolutions_Desafio.Domain.Abstractions;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Utils;

namespace AllRiskSolutions_Desafio.Domain.Repositories;

public interface ICityRepository : IRepository<City>
{
    public Task<bool> ExistsByNameAsync(string name);

    public Task<Result<City>> FindByNameAsync(string name);
}