using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllRiskSolutions_Desafio.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckHealthController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult CheckHealth()
    {
        return Ok();
    }
}