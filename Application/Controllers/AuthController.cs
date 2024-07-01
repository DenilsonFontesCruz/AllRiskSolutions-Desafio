using AllRiskSolutions_Desafio.Domain.Services;
using AllRiskSolutions_Desafio.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllRiskSolutions_Desafio.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(AuthService authService, ILogger<UserController> logger)
    : ControllerBase
{
    public record LoginRequest(string Username, string Password);

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await authService.AuthenticateAsync(request.Username, request.Password);

            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error authenticating user");
            throw;
        }
    }

    [HttpGet("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        try
        {
            var accessToken = Request.Headers.Authorization.ToString().GetBearerToken();
            return await authService.LogoutAsync(accessToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error logging out user");
            throw;
        }
    }
}