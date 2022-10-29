using MovieCatalogApi.Exceptions;

namespace MovieCatalogApi.Middwares;

public static class MiddlewareExtensions
{
    public static void UseExceptionHandlingMiddlwares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UserNotFoundException exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (MovieNotFoundException exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
    }
}