using Microsoft.EntityFrameworkCore;

namespace MovieCatalogApi.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}