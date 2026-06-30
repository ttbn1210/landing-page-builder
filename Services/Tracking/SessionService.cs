using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Services.Tracking;

public class SessionService
{
    private readonly AppDbContext _db;
    private readonly DeviceFingerprintService _fingerprintService;

    public SessionService(AppDbContext db, DeviceFingerprintService fingerprintService)
    {
        _db = db;
        _fingerprintService = fingerprintService;
    }

    public async Task<VisitorSession> CreateOrGetSessionAsync(Guid qrCodeId, string? ipAddress, string? userAgent)
    {
        var fingerprint = _fingerprintService.GenerateFingerprint(ipAddress, userAgent);

        var existingSession = await _db.VisitorSessions
            .FirstOrDefaultAsync(s => s.QRCodeId == qrCodeId && s.DeviceFingerprint == fingerprint);

        if (existingSession != null)
        {
            existingSession.LastSeenAt = DateTime.UtcNow;
            existingSession.IsUniqueVisitor = false;
            await _db.SaveChangesAsync();
            return existingSession;
        }

        var deviceType = _fingerprintService.DetectDeviceType(userAgent);
        var sessionKey = GenerateSessionKey();

        var session = new VisitorSession
        {
            QRCodeId = qrCodeId,
            SessionKey = sessionKey,
            DeviceFingerprint = fingerprint,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            DeviceType = deviceType,
            VisitorSource = VisitorSource.QRScan,
            IsUniqueVisitor = true,
            LastSeenAt = DateTime.UtcNow
        };

        _db.VisitorSessions.Add(session);
        await _db.SaveChangesAsync();

        return session;
    }

    private static string GenerateSessionKey()
    {
        return $"sess_{Guid.NewGuid():N}";
    }
}
