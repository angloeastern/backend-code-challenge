using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading.Tasks;

public class ApiKeyMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private const string ApiKeyHeaderName = "AE_Key";

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        // Bypass API Key check for Swagger and Root
        var path = context.Request.Path;
        if (path.StartsWithSegments("/swagger") || path.Value == "/")
        {
            await _next(context);
            return;
        }

        async Task WriteJsonResponse(string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                status = StatusCodes.Status401Unauthorized,
                message
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }


        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var receivedApiKey))
        {

            await WriteJsonResponse("Authentication Failed. API Key header was not provided.");
            return;
        }


        var expectedApiKey = configuration.GetValue<string>("ApiKeys:AE_Key");


        if (!receivedApiKey.Equals(expectedApiKey))
        {

            await WriteJsonResponse("Authentication Failed. Invalid API Key.");
            return;
        }


        await _next(context);
    }
}