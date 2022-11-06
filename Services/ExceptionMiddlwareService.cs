using Microsoft.AspNetCore.Http.Extensions;
using MovieCatalogApi.Controllers;
using MovieCatalogApi.Exceptions;

namespace MovieCatalogApi.Services;

public class ExceptionMiddlewareService
{
    private readonly RequestDelegate _next;
    private readonly ILoggerService _loggerService;

    public ExceptionMiddlewareService(RequestDelegate next, ILoggerService loggerService)
    {
        _next = next;
        _loggerService = loggerService;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException exception)
        {
            await _loggerService.LogException(MakeLogMessage(context, exception));
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (WrongLoginCredentialsException exception)
        {
            await _loggerService.LogException(MakeLogMessage(context, exception));
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (ReviewAlreadyExistsException exception)
        {
            await _loggerService.LogException(MakeLogMessage(context, exception));
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (PermissionDeniedException exception)
        {
            await _loggerService.LogException(MakeLogMessage(context, exception));
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (BadRequestException exception)
        {
            await _loggerService.LogException(MakeLogMessage(context, exception));
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (UserAlreadyExistsException exception)
        {
            await _loggerService.LogException(MakeLogMessage(context, exception));
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (CanNotGetTokenException exception)
        {
            await _loggerService.LogException(MakeLogMessage(context, exception));
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (NotEnoughtRightsException exception)
        {
            await _loggerService.LogException(MakeLogMessage(context, exception));
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (NotAuthorizedException exception)
        {
            await _loggerService.LogException(MakeLogMessage(context, exception));
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (Exception exception)
        {
            await _loggerService.LogException(MakeLogMessage(context, exception));
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "Something bad happens :(" });
        }
    }

    private static string MakeLogMessage(HttpContext context, Exception exception)
    {
        var message = $"Url: {{{context.Request.GetDisplayUrl()}}}; " +
                      $"Method: {{{context.Request.Method}}}; " +
                      $"Protocol: {{{context.Request.Protocol}}}; " +
                      $"Exception Message: {{{exception.Message}}}; " +
                      $"Stack Trace: {{{exception.StackTrace}}}\n\n";

        return message;
    }
}