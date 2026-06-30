using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Features.Tracking;

public class CaptureTrackingEventHandler
{
    private readonly AppDbContext _db;

    public CaptureTrackingEventHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TrackingEvent?> HandleAsync(string sessionKey, EventType eventType, string? elementName, string? value)
    {
        var session = await _db.VisitorSessions
            .FirstOrDefaultAsync(s => s.SessionKey == sessionKey);

        if (session == null) return null;

        session.LastSeenAt = DateTime.UtcNow;

        var trackingEvent = new TrackingEvent
        {
            SessionId = session.Id,
            EventType = eventType,
            ElementName = elementName,
            Value = value,
            OccurredAt = DateTime.UtcNow
        };

        _db.TrackingEvents.Add(trackingEvent);
        await _db.SaveChangesAsync();

        return trackingEvent;
    }
}
