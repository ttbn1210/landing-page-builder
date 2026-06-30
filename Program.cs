using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Data.Seed;
using TrackQR.Web.Features.Analytics;
using TrackQR.Web.Features.Campaigns;
using TrackQR.Web.Features.Leads;
using TrackQR.Web.Features.LandingPages;
using TrackQR.Web.Features.Placements;
using TrackQR.Web.Features.QRCodes;
using TrackQR.Web.Features.Tracking;
using TrackQR.Web.Infrastructure.Cache;
using TrackQR.Web.Infrastructure.Jobs;
using TrackQR.Web.Infrastructure.Repositories;
using TrackQR.Web.Middleware;
using TrackQR.Web.Models;
using TrackQR.Web.Services.Analytics;
using TrackQR.Web.Services.Landing;
using TrackQR.Web.Services.Leads;
using TrackQR.Web.Services.QR;
using TrackQR.Web.Services.Tracking;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Memory Cache
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MemoryCacheService>();

// Services
builder.Services.AddScoped<QRCodeGeneratorService>();
builder.Services.AddScoped<QRImageService>();
builder.Services.AddScoped<DeviceFingerprintService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<GeoLocationService>();
builder.Services.AddScoped<HtmlRenderService>();
builder.Services.AddScoped<TemplateService>();
builder.Services.AddScoped<ScriptInjectorService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<FunnelService>();
builder.Services.AddScoped<LeadScoreService>();

// Repositories
builder.Services.AddScoped<CampaignRepository>();
builder.Services.AddScoped<QRCodeRepository>();
builder.Services.AddScoped<LeadRepository>();
builder.Services.AddScoped<TrackingRepository>();

// Feature Handlers
builder.Services.AddScoped<CreateCampaignHandler>();
builder.Services.AddScoped<UpdateCampaignHandler>();
builder.Services.AddScoped<ArchiveCampaignHandler>();
builder.Services.AddScoped<CreatePlacementHandler>();
builder.Services.AddScoped<UpdatePlacementHandler>();
builder.Services.AddScoped<CreateLandingPageHandler>();
builder.Services.AddScoped<PublishLandingPageHandler>();
builder.Services.AddScoped<RenderLandingPageHandler>();
builder.Services.AddScoped<GenerateQRCodeHandler>();
builder.Services.AddScoped<BatchGenerateHandler>();
builder.Services.AddScoped<ResolveQRCodeHandler>();
builder.Services.AddScoped<CaptureScanHandler>();
builder.Services.AddScoped<CaptureSessionHandler>();
builder.Services.AddScoped<CaptureTrackingEventHandler>();
builder.Services.AddScoped<DetectDeviceHandler>();
builder.Services.AddScoped<CaptureLeadHandler>();
builder.Services.AddScoped<UpdateLeadStatusHandler>();
builder.Services.AddScoped<AddLeadActivityHandler>();
builder.Services.AddScoped<CampaignReportHandler>();
builder.Services.AddScoped<QRPerformanceHandler>();
builder.Services.AddScoped<ConversionReportHandler>();

// Background Jobs
builder.Services.AddHostedService<AnalyticsAggregationJob>();

// Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<RateLimitMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// =============================================
// TRACKING API ENDPOINTS (minimal API)
// =============================================

app.MapPost("/api/track/event", async (HttpContext context, CaptureTrackingEventHandler handler) =>
{
    var body = await context.Request.ReadFromJsonAsync<TrackEventRequest>();
    if (body == null || string.IsNullOrEmpty(body.SessionKey))
        return Results.BadRequest();

    if (!Enum.TryParse<EventType>(body.EventType, true, out var eventType))
        return Results.BadRequest("Invalid event type");

    var result = await handler.HandleAsync(body.SessionKey, eventType, body.ElementName, body.Value);
    return result != null ? Results.Ok() : Results.NotFound();
});

app.MapPost("/api/track/lead", async (HttpContext context, CaptureLeadHandler handler) =>
{
    var body = await context.Request.ReadFromJsonAsync<CaptureLeadRequest>();
    if (body == null || string.IsNullOrEmpty(body.SessionKey))
        return Results.BadRequest();

    var lead = await handler.HandleAsync(body.SessionKey, body.FullName, body.PhoneNumber, body.Email, body.FormDataJson, body.FormName, body.FormType);
    return lead != null ? Results.Ok(new { lead.Id }) : Results.NotFound();
});

// =============================================
// PUBLIC LANDING PAGE ROUTING
// =============================================

app.MapGet("/{slug}", async (string slug, HttpContext context, RenderLandingPageHandler handler) =>
{
    // Skip admin/static file routes
    if (slug.StartsWith("api") || slug.StartsWith("admin") || slug.Contains('.'))
        return Results.NotFound();

    var ipAddress = context.Connection.RemoteIpAddress?.ToString();
    var userAgent = context.Request.Headers.UserAgent.ToString();
    var referrer = context.Request.Headers.Referer.ToString();

    var result = await handler.HandleAsync(slug, ipAddress, userAgent, referrer);

    if (result == null)
        return Results.NotFound("QR Code not found or inactive.");

    return Results.Content(result.Html, "text/html");
});

// Razor Pages (Admin)
app.MapRazorPages();

// Database initialization
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    SeedData.Initialize(db);
}

app.Run();

// Request models for minimal API
public record TrackEventRequest(string SessionKey, string EventType, string? ElementName, string? Value);
public record CaptureLeadRequest(string SessionKey, string? FullName, string? PhoneNumber, string? Email, string? FormDataJson, string? FormName, string? FormType);
