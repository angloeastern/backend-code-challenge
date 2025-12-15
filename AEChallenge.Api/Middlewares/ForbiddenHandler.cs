using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Text.Json;

namespace AEChallenge.Api.Middlewares
{
    public class ForbiddenHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {

            if (authorizeResult.Forbidden)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";


                string requiredRole = context.GetEndpoint()
                                             ?.Metadata
                                             .GetMetadata<AuthorizeAttribute>()
                                             ?.Roles ?? "A required role";

                var errorResponse = new
                {
                    status = StatusCodes.Status403Forbidden,
                    message = "Access Denied. You do not have the required role to access this resource. Roles : " + requiredRole
                };

                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);

                return; // Menghentikan pemrosesan 403 oleh handler default
            }


            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
