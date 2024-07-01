using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AllRiskSolutions_Desafio.Configuration;
using AllRiskSolutions_Desafio.Domain.Abstractions;
using AllRiskSolutions_Desafio.Domain.ExternalServices;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Domain.Repositories;
using AllRiskSolutions_Desafio.Utils;
using Microsoft.IdentityModel.Tokens;

namespace AllRiskSolutions_Desafio.Domain.Services;

public class AuthService(
    ICacheManager blockListCacheManager,
    IEncryptor encryptor,
    IUserRepository userRepository,
    VariableProvider variableProvider,
    JwtSecurityTokenHandler jwtSecurityTokenHandler,
    ILogger<AuthService> logger)
{
    public async Task<Result<string>> AuthenticateAsync(string username, string password)
    {
        logger.LogInformation("Authenticating user");
        var userResult = await userRepository.FindByUsernameAsync(username);

        if (userResult.IsFail)
        {
            return Result.Fail<string>(userResult.Error);
        }

        var user = userResult.Value();

        if (!await encryptor.CompareAsync(password, user.Password))
        {
            logger.LogInformation("Invalid credentials");
            return Result.Fail<string>("Invalid credentials", "401");
        }

        return $"Bearer {CreateToken(user.Id)}";
    }

    public async Task<Result> LogoutAsync(string accessToken)
    {
        logger.LogInformation("Logging out user");
        var accessTokenBody = jwtSecurityTokenHandler.ReadToken(accessToken);
        var expirationTime =
            TimeSpan.FromTicks(accessTokenBody.ValidTo.Ticks - DateTime.UtcNow.Ticks);
        await blockListCacheManager.SetAsync(accessToken, string.Empty, expirationTime);

        return Result.Success();
    }

    private string CreateToken(Guid id)
    {
        var key = Encoding.ASCII.GetBytes(variableProvider.GetJwtSecret());
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, id.ToString()),
                new Claim(ClaimTypes.Role, "User")
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

        return jwtSecurityTokenHandler.WriteToken(token);
    }
}