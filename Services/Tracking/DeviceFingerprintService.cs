using TrackQR.Web.Models;
using UAParser;

namespace TrackQR.Web.Services.Tracking;

public class DeviceFingerprintService
{
    private static readonly Parser _parser = Parser.GetDefault();

    public DeviceType DetectDeviceType(string? userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
            return DeviceType.Unknown;

        var clientInfo = _parser.Parse(userAgent);
        var device = clientInfo.Device.Family?.ToLowerInvariant() ?? "";

        if (device.Contains("iphone") || device.Contains("android") ||
            userAgent.Contains("Mobile", StringComparison.OrdinalIgnoreCase))
            return DeviceType.Mobile;

        if (device.Contains("ipad") || userAgent.Contains("Tablet", StringComparison.OrdinalIgnoreCase))
            return DeviceType.Tablet;

        return DeviceType.Desktop;
    }

    public string GenerateFingerprint(string? ipAddress, string? userAgent)
    {
        var raw = $"{ipAddress}|{userAgent}";
        var bytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes)[..32].ToLowerInvariant();
    }
}
