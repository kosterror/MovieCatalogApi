using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;

namespace MovieCatalogApi.Services;

public class ValidateTokenService : IValidateTokenService
{
    private readonly ApplicationDbContext _context;

    public ValidateTokenService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ValidateToken(IHeaderDictionary headerDictionary)
    {
        //достать токен
        var token = GetToken(headerDictionary);

        var tokenEntities = await _context
            .Tokens
            .Where(x => x.Token == token)
            .FirstOrDefaultAsync();

        if (tokenEntities != null)
        {
            throw new UserNotFoundException("Token expired");
        }
        
    }
    
    /**
     * достаем токен из хэдера запроса
     */
    private string GetToken(IHeaderDictionary headerDictionary)
    {
        var requestHeaders = new Dictionary<string, string>();

        foreach (var header in headerDictionary)
        {
            requestHeaders.Add(header.Key, header.Value);
        }

        var authorizationString = requestHeaders["Authorization"];


        const string pattern = @"\S+\.\S+\.\S+";
        var regex = new Regex(pattern);
        var matches = regex.Matches(authorizationString);

        if (matches.Count <= 0)
        {
            throw new CanNotGetTokenException("Can not get the token from headers");
        }

        return matches[0].Value;
    }
}