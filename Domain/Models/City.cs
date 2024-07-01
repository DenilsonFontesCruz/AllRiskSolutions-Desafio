using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AllRiskSolutions_Desafio.Domain.Abstractions;
using AllRiskSolutions_Desafio.Utils;

namespace AllRiskSolutions_Desafio.Domain.Models;

public class City(Guid id, string name, double latitude, double longitude, string countryCode)
    : Model(id)
{
    [Required] [StringLength(80)] public string Name { get; set; } = name;
    [Required] public double Latitude { get; set; } = latitude;
    [Required] public double Longitude { get; set; } = longitude;
    [Required] [StringLength(3)] public string CountryCode { get; set; } = countryCode;
    [Required] public DateTime CreatedAt { get; init; } = DateTime.Now;

    public City(Guid id, string name, Coords coords, string countryCode)
        : this(id, name, coords.Latitude, coords.Longitude, countryCode)
    {
    }
}

public record Coords(double Latitude, double Longitude);

public record CityViewModel(Guid Id, string Name, Coords Coords, string CountryCode)
{
    public static implicit operator CityViewModel(City city)
    {
        return new CityViewModel(city.Id, city.Name, new Coords(city.Latitude, city.Longitude),
            city.CountryCode);
    }

    public static Result<CityViewModel> ToResult(Result<City> result)
    {
        return result.IsSuccess
            ? Result.Success<CityViewModel>(result.Value())
            : Result.Fail<CityViewModel>(result.Error);
    }
}