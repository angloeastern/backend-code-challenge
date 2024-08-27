
using AEBackend.Middlewares;

namespace AEBackend.Extensions;
// Extension method used to add the middleware to the HTTP request pipeline.
public static class ExceptionHandlerMiddlewareExtensions
{
  public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<ExceptionHandlerMiddleware>();
  }
}