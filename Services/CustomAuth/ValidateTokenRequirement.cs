using Microsoft.AspNetCore.Authorization;

namespace MovieCatalogApi.Services.CustomAuth;

public class ValidateTokenRequirement : IAuthorizationRequirement
{
    public ValidateTokenRequirement()
    {
    }
}