using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Infrastructure.Cache;

namespace TrackQR.Web.Infrastructure.Jobs;

public class AnalyticsAggregationJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AnalyticsAggregationJob> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

    public AnalyticsAggregationJob(IServiceProvider serviceProvider, ILogger<AnalyticsAggregationJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await AggregateAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during analytics aggregation");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task AggregateAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var cache = scope.ServiceProvider.GetRequiredService<MemoryCacheService>();

        // Mark converted sessions
        var convertedSessionIds = await db.Leads
            .Select(l => l.SessionId)
            .ToListAsync(ct);

        var sessions = await db.VisitorSessions
            .Where(s => convertedSessionIds.Contains(s.Id) && !s.IsConverted)
            .ToListAsync(ct);

        foreach (var session in sessions)
        {
            session.IsConverted = true;
        }

        if (sessions.Count > 0)
        {
            await db.SaveChangesAsync(ct);
            _logger.LogInformation("Marked {Count} sessions as converted", sessions.Count);
        }

        // Invalidate dashboard cache
        cache.Remove(CacheKeys.DashboardStats);
    }
}
