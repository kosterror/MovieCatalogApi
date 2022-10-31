namespace MovieCatalogApi.Exceptions;

public class CanNotGetTokenException : Exception
{
    public CanNotGetTokenException(string message) : base(message)
    {
    }
}