namespace MovieCatalogApi.Controllers;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message) : base(message)
    {
    }
}