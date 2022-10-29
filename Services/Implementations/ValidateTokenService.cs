using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Models;

namespace MovieCatalogApi.Services.Implementations;

public class ValidateTokenService : IValidateTokenService
{
    private readonly ApplicationDbContext _context;

    public ValidateTokenService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsValidToken(IHeaderDictionary headerDictionary)
    {
        //достать токен
        var token = await GetToken(headerDictionary);

        var tokenEntities = await _context.Tokens.Where(x => x.Token == token).ToListAsync();

        return tokenEntities.Count <= 0 && token != "";
    }

    public async Task<string> GetToken(IHeaderDictionary headerDictionary)
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
            throw new Exception();
        }

        return matches.Count > 0 ? matches[0].Value : "";
    }
}