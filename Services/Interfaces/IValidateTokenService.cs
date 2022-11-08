namespace MovieCatalogApi.Services;

public interface IValidateTokenService
{
    Task ValidateToken(IHeaderDictionary headerDictionary);
}