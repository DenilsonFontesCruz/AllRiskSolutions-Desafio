using AllRiskSolutions_Desafio.Domain.Abstractions;
using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Domain.Services;
using AllRiskSolutions_Desafio.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllRiskSolutions_Desafio.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(
    UserService userService,
    CityService cityService,
    ILogger<UserController> logger)
    : ControllerBase
{
    public record RegisterRequest(string Username, string Password);

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserViewModel>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = UserViewModel.ToResult(
                await userService.RegisterUserAsync(request.Username, request.Password));

            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error registering user");
            throw;
        }
    }

    [HttpGet("info")]
    [Authorize]
    public async Task<ActionResult<UserViewModel>> GetUserInfo()
    {
        try
        {
            var result =
                UserViewModel.ToResult(
                    await userService.GetByIdWithCities(Guid.Parse(User.Identity!.Name!)));

            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting user info");
            throw;
        }
    }

    public record AddFavoriteCityRequest(string Name, Coords Coords, string CountryCode);

    [HttpPost("add-favorite-city")]
    [Authorize]
    public async Task<ActionResult<UserViewModel>> AddFavoriteCity(
        [FromBody] AddFavoriteCityRequest request)
    {
        try
        {
            City city;
            var cityRegisterResult =
                await cityService.Register(request.Name, request.CountryCode, request.Coords);

            if (cityRegisterResult.IsFail)
            {
                var cityResult = await cityService.GetByName(request.Name);
                if (cityResult.IsFail)
                    return Result.Fail<UserViewModel>(cityResult.Error);

                city = cityResult.Value();
            }
            else city = cityRegisterResult.Value();

            var userResult =
                await userService.AddFavoriteCity(Guid.Parse(User.Identity!.Name!), city);

            return UserViewModel.ToResult(userResult);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error adding favorite city");
            throw;
        }
    }

    public record RemoveFavoriteCityRequest(Guid Id);

    [HttpPost("remove-favorite-city")]
    [Authorize]
    public async Task<ActionResult<UserViewModel>> RemoveFavoriteCity(
        [FromBody] RemoveFavoriteCityRequest request)
    {
        try
        {
            var cityResult = await cityService.GetById(request.Id);
            if (cityResult.IsFail)
            {
                return Result.Fail<UserViewModel>(cityResult.Error);
            }

            var userResult = await userService.RemoveFavoriteCity(Guid.Parse(User.Identity!.Name!),
                cityResult.Value());

            return UserViewModel.ToResult(userResult);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error removing favorite city");
            throw;
        }
    }
}