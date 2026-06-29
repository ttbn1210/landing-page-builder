using System.Collections.Concurrent;

namespace QrCommerce.Web.Helpers;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, (int Count, DateTime ResetAt)> _clients = new();
    private const int MaxRequests = 100;
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);

    public RateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await _next(context);
            return;
        }

        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        var entry = _clients.GetOrAdd(clientIp, _ => (0, now.Add(Window)));

        if (now > entry.ResetAt)
        {
            entry = (1, now.Add(Window));
            _clients[clientIp] = entry;
        }
        else if (entry.Count >= MaxRequests)
        {
            context.Response.StatusCode = 429;
            await context.Response.WriteAsJsonAsync(new { error = "Rate limit exceeded" });
            return;
        }
        else
        {
            _clients[clientIp] = (entry.Count + 1, entry.ResetAt);
        }

        await _next(context);
    }
}
