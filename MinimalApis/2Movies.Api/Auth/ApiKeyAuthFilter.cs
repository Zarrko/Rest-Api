using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _2Movies.Api.Auth;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key is Missing");
            return;
        }

        var apiKey = _configuration["ApiKey"];
        if (apiKey != extractedApiKey)
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
        }
    }
}