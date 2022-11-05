using Microsoft.AspNetCore.Authorization;
using MovieCatalogApi.Models;
using MovieCatalogApi.Services;

namespace MovieCatalogApi.Configurations;

public class CustomAuthRequirement : AuthorizationHandler<CustomAuthRequirement>, IAuthorizationRequirement
{
    private readonly ApplicationDbContext _context;
    private readonly IValidateTokenService _service;
    
    public CustomAuthRequirement(IValidateTokenService service)
    {
        _service = service;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CustomAuthRequirement requirement)
    {
        //бизнес логика
        return Task.CompletedTask;
    }
}