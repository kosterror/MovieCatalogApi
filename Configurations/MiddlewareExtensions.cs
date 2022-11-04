using MovieCatalogApi.Services;

namespace MovieCatalogApi.Configurations;

public static class MiddlewareExtensions
{
    public static void UseExceptionHandlingMiddlwares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddlewareService>();
    }
}