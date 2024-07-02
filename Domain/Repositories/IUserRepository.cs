using AllRiskSolutions_Desafio.Domain.Abstractions;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Utils;

namespace AllRiskSolutions_Desafio.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    public Task<bool> ExistsByUsernameAsync(string username);

    public Task<Result<User>> FindByUsernameAsync(string username);

    public User? Update(User model);

    public Task<Result<User>> FindByIdWithCitiesAsync(Guid id);
}