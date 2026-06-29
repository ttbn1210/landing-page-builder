using QrCommerce.Web.Services;

namespace QrCommerce.Web.Helpers;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/device"))
        {
            if (context.Request.Path.Value?.EndsWith("/register") == true)
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var apiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { error = "API key required" });
                return;
            }

            using var scope = context.RequestServices.CreateScope();
            var deviceService = scope.ServiceProvider.GetRequiredService<DeviceService>();
            var device = await deviceService.ValidateApiKeyAsync(apiKey.ToString());

            if (device == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid API key" });
                return;
            }

            context.Items["Device"] = device;
        }

        await _next(context);
    }
}
