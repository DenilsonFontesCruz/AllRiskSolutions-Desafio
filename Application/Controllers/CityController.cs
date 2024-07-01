using AllRiskSolutions_Desafio.Domain.Models;
using AllRiskSolutions_Desafio.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllRiskSolutions_Desafio.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class CityController(CityService cityService, ILogger<CityController> logger)
    : ControllerBase
{
    [HttpGet($"search")]
    [AllowAnonymous]
    public async Task<ActionResult<CityViewModel>> SearchCity([FromQuery] string name)
    {
        try
        {
            var result = CityViewModel.ToResult(await cityService.GetByName(name));

            if (result.IsFail)
            {
                return result;
            }

            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error searching city");
            throw;
        }
    }
}