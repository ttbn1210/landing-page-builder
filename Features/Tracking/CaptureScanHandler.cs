using TrackQR.Web.Data;
using TrackQR.Web.Models;
using TrackQR.Web.Services.Tracking;

namespace TrackQR.Web.Features.Tracking;

public class CaptureScanHandler
{
    private readonly AppDbContext _db;
    private readonly GeoLocationService _geoService;

    public CaptureScanHandler(AppDbContext db, GeoLocationService geoService)
    {
        _db = db;
        _geoService = geoService;
    }

    public async Task<ScanEvent> HandleAsync(Guid qrCodeId, Guid sessionId, string? ipAddress, string? referrer)
    {
        var geo = _geoService.Resolve(ipAddress);

        var scanEvent = new ScanEvent
        {
            QRCodeId = qrCodeId,
            SessionId = sessionId,
            Country = geo.Country,
            Province = geo.Province,
            City = geo.City,
            Referrer = referrer,
            OccurredAt = DateTime.UtcNow
        };

        _db.ScanEvents.Add(scanEvent);
        await _db.SaveChangesAsync();

        return scanEvent;
    }
}
