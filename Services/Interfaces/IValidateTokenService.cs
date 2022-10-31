namespace MovieCatalogApi.Services;

public interface IValidateTokenService
{
    void ValidateToken(IHeaderDictionary headerDictionary);
    string GetToken(IHeaderDictionary headerDictionary);
}