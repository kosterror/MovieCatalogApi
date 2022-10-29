namespace MovieCatalogApi.Exceptions;

public class WrongLoginCredentialsException : Exception
{
    public WrongLoginCredentialsException(string message) : base(message)
    {
    }
}