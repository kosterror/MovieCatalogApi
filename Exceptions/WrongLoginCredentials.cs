namespace MovieCatalogApi.Exceptions;

public class WrongLoginCredentials : Exception
{
    public WrongLoginCredentials(string message) : base(message)
    {
    }
}