using System.ComponentModel.DataAnnotations;
using AllRiskSolutions_Desafio.Domain.Abstractions;
using AllRiskSolutions_Desafio.Utils;

namespace AllRiskSolutions_Desafio.Domain.Models;

public class User(
    Guid id,
    string username,
    string password,
    ICollection<City> favoriteCities)
    : Model(id)
{
    [Required] [StringLength(40)] public string Username { get; set; } = username;
    [Required] [StringLength(72)] public string Password { get; set; } = password;
    public ICollection<City> FavoriteCities { get; set; } = favoriteCities;
    [Required] public DateTime CreatedAt { get; init; } = DateTime.Now;

    public User(Guid id, string username, string password) : this(id, username,
        password, [])
    {
    }

    public Result<User> AddFavoriteCity(City city)
    {
        if (FavoriteCities.Contains(city))
        {
            return Result.Fail<User>("City already favorite", "409");
        }

        FavoriteCities = FavoriteCities.Append(city).ToList();
        return Result.Success(this);
    }

    public Result<User> RemoveFavoriteCity(City city)
    {
        if (!FavoriteCities.Contains(city))
        {
            return Result.Fail<User>("City not favorite", "404");
        }

        FavoriteCities = FavoriteCities.Where(c => c != city).ToList();
        return Result.Success(this);
    }
}

public record UserViewModel(
    Guid Id,
    string Username,
    IEnumerable<CityViewModel> FavoriteCities
)
{
    public static implicit operator UserViewModel(User user)
    {
        var favoriteCities = user.FavoriteCities.Select(
            city => (CityViewModel)city);
        return new UserViewModel(user.Id, user.Username, favoriteCities);
    }

    public static Result<UserViewModel> ToResult(Result<User> result)
    {
        return result.IsSuccess
            ? Result.Success<UserViewModel>((result.Value()))
            : Result.Fail<UserViewModel>(result.Error);
    }
}