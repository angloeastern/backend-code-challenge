
using AEBackend.Controllers.Utils;

namespace AEBackend.Middlewares;

public class ExceptionHandlerMiddleware
{
  private readonly RequestDelegate _next;

  public ExceptionHandlerMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext httpContext)
  {
    try
    {
      await _next(httpContext);
    }
    catch (Exception ex)
    {
      await HandleException(ex, httpContext);
    }
  }

  private async Task HandleException(Exception ex, HttpContext httpContext)
  {


    if (ex is InvalidOperationException)
    {
      httpContext.Response.StatusCode = 500;
      await httpContext.Response.WriteAsJsonAsync(ApiResult.Failure(new ApiError("Invalid operation")));
    }
    else if (ex is ArgumentException)
    {
      await httpContext.Response.WriteAsync("Invalid argument");
    }
    else
    {
      await httpContext.Response.WriteAsync("Unknown error");
    }


  }
}

