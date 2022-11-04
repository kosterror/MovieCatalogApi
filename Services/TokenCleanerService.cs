using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Models;

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
            var currentDateTime = DateTime.UtcNow;

            var expiredTokens = await _context.Tokens.Where(x => x.ExpiredDate <= currentDateTime).ToListAsync(cancellationToken: stoppingToken);

            foreach (var expiredToken in expiredTokens)
            {
                _context.Tokens.Remove(expiredToken);
                Console.WriteLine(expiredToken.ExpiredDate);
            }

            await _context.SaveChangesAsync(stoppingToken);
            
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
       }
    }
}