namespace MovieCatalogApi.Exceptions;

public class MovieNotFoundException : Exception
{
    public MovieNotFoundException(string message) : base(message)
    {
    }
}