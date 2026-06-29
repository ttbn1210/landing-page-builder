namespace QrCommerce.Shared.DTOs;

public class DeviceHeartbeatDto
{
    public string DeviceCode { get; set; } = string.Empty;
    public int Battery { get; set; }
    public string Network { get; set; } = string.Empty;
}
