namespace MovieCatalogApi.Services;

public interface ILoggerService
{
    Task LogInfo(string message);

    Task LogException(string message);
}