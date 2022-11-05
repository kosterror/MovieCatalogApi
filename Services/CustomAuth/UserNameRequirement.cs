using Microsoft.AspNetCore.Authorization;

namespace MovieCatalogApi.Services.CustomAuth;

public class UserNameRequirement : IAuthorizationRequirement
{
    public UserNameRequirement(string userName)
    {
        UserName = userName;
    }

    public string UserName { get; }
}