using AllRiskSolutions_Desafio.Configuration;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Domain.Repositories;
using AllRiskSolutions_Desafio.Utils;
using Microsoft.EntityFrameworkCore;

namespace AllRiskSolutions_Desafio.Infrastructure.Repositories;

public class UserRepositorySql(AppDbContext context, ILogger<UserRepositorySql> logger)
    : IUserRepository
{
    public async Task<User?> InsertAsync(User model)
    {
        var result = await context.Users.AddAsync(model);
        return result.Entity;
    }

    public async Task InsertManyAsync(IEnumerable<User> models)
    {
        await context.Users.AddRangeAsync(models);
    }

    public async Task<Result<User>> FindByIdAsync(Guid id)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            logger.LogInformation($"User not found, Id: {id.ToString()}");
            return Result.Fail<User>("User not found", "404");
        }

        return user;
    }

    public async Task<Result<IEnumerable<User>>> FindAllAsync()
    {
        var users = await context.Users.ToListAsync();
        return users;
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await context.Users.AnyAsync(x => x.Username == username);
    }

    public async Task<Result<User>> FindByUsernameAsync(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (user == null)
        {
            logger.LogInformation($"User not found, Username: {username}");
            return Result.Fail<User>("User not found", "404");
        }

        return user;
    }

    public User? Update(User model)
    {
        var user = context.Users.Update(model);
        return user.Entity;
    }

    public async Task<Result<User>> FindByIdWithCitiesAsync(Guid id)
    {
        var user = await context.Users.Include("FavoriteCities")
            .FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            logger.LogInformation($"User not found, Id: {id.ToString()}");
            return Result.Fail<User>("User not found", "404");
        }

        return user;
    }

    public async Task<int> SaveAsync()
    {
        return await context.SaveChangesAsync();
    }
}