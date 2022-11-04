﻿using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Models;
using MovieCatalogApi.Models.Entities;

namespace MovieCatalogApi.Services;

public class TokenCleanerService : BackgroundService
{
    private readonly ApplicationDbContext _context;
    private readonly ILoggerService _loggerService;
    private readonly TimeSpan _delay;

    public TokenCleanerService(ApplicationDbContext context, ILoggerService loggerService, IConfiguration configuration)
    {
        //нет смысла держать configuration свойством класса, т.к. мы к обращаемся к нему единожды
        _loggerService = loggerService;
        _delay = TimeSpan.FromMinutes(configuration.GetValue<int>("TokenCleanerDelayInMinutes"));   
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var currentDateTime = DateTime.UtcNow;

            var expiredTokens = await _context
                .Tokens
                .Where(x => x.ExpiredDate <= currentDateTime)
                .ToListAsync(cancellationToken: stoppingToken);

            await _loggerService.LogInfo(MakeMessage(expiredTokens));
            
            foreach (var expiredToken in expiredTokens)
            {
                _context.Tokens.Remove(expiredToken);
            }

            await _context.SaveChangesAsync(stoppingToken);
            
            await Task.Delay(_delay, stoppingToken);
        }
    }

    private static string MakeMessage(List<TokenEntity> tokenEntities)
    {
        var message = "Token cleaner worked. Removed tokens: {";
        
        foreach (var tokenEntity in tokenEntities)
        {
            message += $"\nToken: {{{tokenEntity.Token}}}; Expired date: {{{tokenEntity.ExpiredDate}}}";
        }
        
        return $"{message}}}";
    }
}