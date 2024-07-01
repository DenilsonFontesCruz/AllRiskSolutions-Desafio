using System.IdentityModel.Tokens.Jwt;
using AllRiskSolutions_Desafio.Domain.ExternalServices;
using AllRiskSolutions_Desafio.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace AllRiskSolutions_Desafio.Configuration;

public class AuthorizationMiddlewareResultHandler(
    ICacheManager cacheManager,
    JwtSecurityTokenHandler jwtSecurityTokenHandler)
    : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next, HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Challenged)
        {
            context.Response.StatusCode = 401;
            return;
        }

        if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = 403;
            return;
        }

        var accessToken = context.Request.Headers.Authorization.ToString().GetBearerToken();
        var tokenExists = await cacheManager.ExistsAsync(accessToken);

        if (tokenExists)
        {
            context.Response.StatusCode = 401;
            return;
        }

        await next(context);
    }
}