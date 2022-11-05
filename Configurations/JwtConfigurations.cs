using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MovieCatalogApi.Conmfigurations;

public class JwtConfigurations
{
    public const string Issuer = "MovieCatalogApi";                         // издатель 
    public const string Audience = "MovieCatalogFrontend";                  // потребитель
    private const string Key = "TheVeryStrongKeyOrPasswordQwerty123";       // ключ для шифрации
    public const int Lifetime = 2;                                          // время жизни токена в минутах
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }

}