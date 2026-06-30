using TrackQR.Web.Models;
using TrackQR.Web.Services.Tracking;

namespace TrackQR.Web.Features.Tracking;

public class DetectDeviceHandler
{
    private readonly DeviceFingerprintService _fingerprintService;

    public DetectDeviceHandler(DeviceFingerprintService fingerprintService)
    {
        _fingerprintService = fingerprintService;
    }

    public DeviceType Handle(string? userAgent)
    {
        return _fingerprintService.DetectDeviceType(userAgent);
    }
}
