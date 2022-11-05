namespace MovieCatalogApi.Exceptions;

public class NotEnoughtRightsException : Exception
{
    public NotEnoughtRightsException(string message) : base(message)
    {
    }
}