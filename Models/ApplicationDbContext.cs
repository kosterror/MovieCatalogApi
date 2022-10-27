using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<MovieEntity> Movies { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<GenreEntity> Genres { get; set; }
    public DbSet<TokenEntity> Tokens { get; set; }
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}