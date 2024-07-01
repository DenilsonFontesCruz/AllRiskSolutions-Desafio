using AllRiskSolutions_Desafio.Utils;

namespace AllRiskSolutions_Desafio.Domain.Abstractions;

public interface IRepository<TModel> where TModel : Model
{
    Task<TModel?> InsertAsync(TModel model);

    Task InsertManyAsync(IEnumerable<TModel> models);

    Task<Result<TModel>> FindByIdAsync(Guid id);

    Task<Result<IEnumerable<TModel>>> FindAllAsync();
    Task<int> SaveAsync();
}