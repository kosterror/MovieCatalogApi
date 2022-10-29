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
        catch (NotFoundException exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (WrongLoginCredentials exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
    }
}