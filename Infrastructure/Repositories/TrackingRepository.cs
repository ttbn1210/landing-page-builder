using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Infrastructure.Repositories;

public class TrackingRepository
{
    private readonly AppDbContext _db;

    public TrackingRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ScanEvent> CreateScanEventAsync(ScanEvent scanEvent)
    {
        _db.ScanEvents.Add(scanEvent);
        await _db.SaveChangesAsync();
        return scanEvent;
    }

    public async Task<TrackingEvent> CreateTrackingEventAsync(TrackingEvent trackingEvent)
    {
        _db.TrackingEvents.Add(trackingEvent);
        await _db.SaveChangesAsync();
        return trackingEvent;
    }

    public async Task<List<TrackingEvent>> GetEventsBySessionAsync(Guid sessionId)
    {
        return await _db.TrackingEvents
            .Where(t => t.SessionId == sessionId)
            .OrderByDescending(t => t.OccurredAt)
            .ToListAsync();
    }

    public async Task<List<ScanEvent>> GetScansByQRCodeAsync(Guid qrCodeId, DateTime? from = null, DateTime? to = null)
    {
        var query = _db.ScanEvents.Where(s => s.QRCodeId == qrCodeId);

        if (from.HasValue)
            query = query.Where(s => s.OccurredAt >= from.Value);
        if (to.HasValue)
            query = query.Where(s => s.OccurredAt <= to.Value);

        return await query.OrderByDescending(s => s.OccurredAt).ToListAsync();
    }
}
