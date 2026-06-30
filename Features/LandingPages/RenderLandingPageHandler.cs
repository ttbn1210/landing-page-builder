using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Infrastructure.Cache;
using TrackQR.Web.Models;
using TrackQR.Web.Services.Landing;
using TrackQR.Web.Services.Tracking;

namespace TrackQR.Web.Features.LandingPages;

public class RenderLandingPageHandler
{
    private readonly AppDbContext _db;
    private readonly HtmlRenderService _renderService;
    private readonly SessionService _sessionService;
    private readonly GeoLocationService _geoService;
    private readonly MemoryCacheService _cache;
    private readonly IConfiguration _config;

    public RenderLandingPageHandler(
        AppDbContext db,
        HtmlRenderService renderService,
        SessionService sessionService,
        GeoLocationService geoService,
        MemoryCacheService cache,
        IConfiguration config)
    {
        _db = db;
        _renderService = renderService;
        _sessionService = sessionService;
        _geoService = geoService;
        _cache = cache;
        _config = config;
    }

    public async Task<RenderResult?> HandleAsync(string shortCode, string? ipAddress, string? userAgent, string? referrer)
    {
        var qrCode = _cache.Get<Models.QRCode>(CacheKeys.QRCode(shortCode));
        if (qrCode == null)
        {
            qrCode = await _db.QRCodes
                .Where(q => q.ShortCode == shortCode && q.IsActive && q.Status == QRCodeStatus.Active)
                .Select(q => q)
                .FirstOrDefaultAsync();

            if (qrCode == null)
                return null;

            _cache.Set(CacheKeys.QRCode(shortCode), qrCode, TimeSpan.FromMinutes(10));
        }

        var landingPage = await _db.LandingPages.FindAsync(qrCode.LandingPageId);
        if (landingPage == null || landingPage.Status != LandingPageStatus.Published)
            return null;

        // Create session
        var session = await _sessionService.CreateOrGetSessionAsync(qrCode.Id, ipAddress, userAgent);

        // Create scan event
        var geo = _geoService.Resolve(ipAddress);
        var scanEvent = new ScanEvent
        {
            QRCodeId = qrCode.Id,
            SessionId = session.Id,
            Country = geo.Country,
            Province = geo.Province,
            City = geo.City,
            Referrer = referrer,
            OccurredAt = DateTime.UtcNow
        };
        _db.ScanEvents.Add(scanEvent);
        await _db.SaveChangesAsync();

        // Render page
        var baseUrl = _config["App:BaseUrl"] ?? "https://localhost:5001";
        var html = _renderService.RenderPage(landingPage, session.SessionKey, baseUrl);

        return new RenderResult { Html = html, SessionKey = session.SessionKey };
    }
}

public class RenderResult
{
    public string Html { get; set; } = string.Empty;
    public string SessionKey { get; set; } = string.Empty;
}
