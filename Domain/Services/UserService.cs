using AllRiskSolutions_Desafio.Domain.ExternalServices;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Domain.Repositories;
using AllRiskSolutions_Desafio.Utils;

namespace AllRiskSolutions_Desafio.Domain.Services;

public class UserService(
    IUserRepository userRepository,
    IEncryptor encryptor,
    ILogger<UserService> logger)
{
    public async Task<Result<User>> RegisterUserAsync(string username, string password)
    {
        logger.LogInformation("Registering user");
        if (await userRepository.ExistsByUsernameAsync(username))
        {
            logger.LogInformation($"Username: {username} already in use");
            return Result.Fail<User>($"Username: {username} already in use", "409");
        }

        var passwordSalt = await encryptor.GenerateSaltAsync(12);
        var encryptedPassword = await encryptor.EncryptAsync(password, passwordSalt);

        var user = new User(Guid.NewGuid(), username, encryptedPassword);

        await userRepository.InsertAsync(user);
        await userRepository.SaveAsync();

        return user;
    }

    public async Task<Result<User>> AddFavoriteCity(Guid userId, City city)
    {
        logger.LogInformation("Adding favorite city");
        var userResult = await userRepository.FindByIdAsync(userId);

        if (userResult.IsFail)
        {
            return userResult;
        }

        var user = userResult.Value();
        var addListResult = user.AddFavoriteCity(city);

        if (addListResult.IsFail)
        {
            return addListResult;
        }

        userRepository.Update(user);
        await userRepository.SaveAsync();

        return user;
    }

    public async Task<Result<User>> RemoveFavoriteCity(Guid userId, City city)
    {
        logger.LogInformation("Removing favorite city");
        var userResult = await userRepository.FindByIdAsync(userId);

        if (userResult.IsFail)
        {
            return userResult;
        }

        var user = userResult.Value();
        var removeListResult = user.RemoveFavoriteCity(city);

        if (removeListResult.IsFail)
        {
            return removeListResult;
        }

        userRepository.Update(user);
        await userRepository.SaveAsync();

        return user;
    }

    public async Task<Result<User>> GetById(Guid id)
    {
        logger.LogInformation("Getting user");
        var userResult = await userRepository.FindByIdAsync(id);

        if (userResult.IsFail)
        {
            return userResult;
        }

        return userResult;
    }
}