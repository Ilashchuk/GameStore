using System.Diagnostics.CodeAnalysis;
using BLL.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Gamestore.Middlewares;

[ExcludeFromCodeCoverage]
public class TotalGamesHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;

    public TotalGamesHeaderMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context, IGameControlService gameControlService)
    {
        const string cacheKey = "TotalGamesCount";
        const int cacheDurationInSeconds = 60;

        if (!_cache.TryGetValue(cacheKey, out int totalGamesCount))
        {
            totalGamesCount = await gameControlService.GetTotalGamesCountAsync();

            _cache.Set(cacheKey, totalGamesCount, TimeSpan.FromSeconds(cacheDurationInSeconds));
        }

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append("x-total-numbers-of-games", totalGamesCount.ToString());
            return Task.CompletedTask;
        });

        await _next(context);
    }
}
