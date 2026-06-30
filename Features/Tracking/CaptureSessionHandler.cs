using TrackQR.Web.Models;
using TrackQR.Web.Services.Tracking;

namespace TrackQR.Web.Features.Tracking;

public class CaptureSessionHandler
{
    private readonly SessionService _sessionService;

    public CaptureSessionHandler(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task<VisitorSession> HandleAsync(Guid qrCodeId, string? ipAddress, string? userAgent)
    {
        return await _sessionService.CreateOrGetSessionAsync(qrCodeId, ipAddress, userAgent);
    }
}
