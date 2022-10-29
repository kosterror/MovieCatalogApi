namespace MovieCatalogApi.Services;

public interface IValidateTokenService
{
    Task<bool> IsValidToken(IHeaderDictionary headerDictionary);
    Task<string> GetToken(IHeaderDictionary headerDictionary);
}