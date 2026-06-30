using System.Diagnostics;

namespace TrackQR.Web.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var path = context.Request.Path;
        var method = context.Request.Method;

        try
        {
            await _next(context);
            sw.Stop();

            _logger.LogInformation("{Method} {Path} → {StatusCode} in {Elapsed}ms",
                method, path, context.Response.StatusCode, sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(ex, "{Method} {Path} → EXCEPTION in {Elapsed}ms",
                method, path, sw.ElapsedMilliseconds);
            throw;
        }
    }
}
