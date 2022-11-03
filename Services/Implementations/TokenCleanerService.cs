using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services.Implementations;

public class TokenCleanerService : BackgroundService
{

    private readonly ApplicationDbContext _context;
    
    public TokenCleanerService(ApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var tokenEntity = new TokenEntity()
            {
                Id = Guid.NewGuid(),
                Token = DateTime.UtcNow.ToLongTimeString()
            };

            _context.Tokens.AddAsync(tokenEntity);
            _context.SaveChangesAsync();
            
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}