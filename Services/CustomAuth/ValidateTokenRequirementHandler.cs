using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Exceptions;
using MovieCatalogApi.Models;

namespace MovieCatalogApi.Services.CustomAuth;

public class ValidateTokenRequirementHandler : AuthorizationHandler<ValidateTokenRequirement>
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ValidateTokenRequirementHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ValidateTokenRequirement requirement)
    {
        var authorizationString = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        var token = GetToken(authorizationString);
        
        var tokenEntity = await _context.Tokens.Where(x => x.Token == token).FirstOrDefaultAsync();

        if (tokenEntity != null)
        {
            throw new NotAuthorizedException("Not authorized");
        }

        context.Succeed(requirement);
    }

    private static string GetToken(string authorizationString)
    {
        const string pattern = @"\S+\.\S+\.\S+";
        var regex = new Regex(pattern);
        var matches = regex.Matches(authorizationString);

        if (matches.Count <= 0)
        {
            throw new NotAuthorizedException("Not authorized");
        }

        var token = matches[0].Value;
        
        if (token == null)
        {
            throw new NotAuthorizedException("Not authorized");
        }

        return token;
    }
}