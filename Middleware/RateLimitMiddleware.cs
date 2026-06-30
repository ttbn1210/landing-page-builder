using System.Collections.Concurrent;

namespace TrackQR.Web.Middleware;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, RateLimitEntry> _clients = new();
    private const int MaxRequests = 60;
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);

    public RateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var path = context.Request.Path.Value ?? "";

        // Only rate limit tracking endpoints
        if (!path.StartsWith("/api/track", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var entry = _clients.GetOrAdd(clientIp, _ => new RateLimitEntry());

        lock (entry)
        {
            if (DateTime.UtcNow - entry.WindowStart > Window)
            {
                entry.WindowStart = DateTime.UtcNow;
                entry.RequestCount = 0;
            }

            entry.RequestCount++;

            if (entry.RequestCount > MaxRequests)
            {
                context.Response.StatusCode = 429;
                return;
            }
        }

        await _next(context);
    }

    private class RateLimitEntry
    {
        public DateTime WindowStart { get; set; } = DateTime.UtcNow;
        public int RequestCount { get; set; }
    }
}
