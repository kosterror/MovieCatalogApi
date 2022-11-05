using Microsoft.AspNetCore.Authorization;
using MovieCatalogApi.Models;

namespace MovieCatalogApi.Services.CustomAuth;

public class UserNameRequirementHandler : AuthorizationHandler<UserNameRequirement>
{
    private readonly ApplicationDbContext _context;

    public UserNameRequirementHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserNameRequirement requirement)
    {
        Console.WriteLine($"\n{DateTime.Now.ToLongTimeString()} | Username:{context.User.Identity.Name}\n");
        
        context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}