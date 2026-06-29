namespace QrCommerce.Shared.Models;

public class TelecomDevice
{
    public int Id { get; set; }
    public string DeviceCode { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Carrier { get; set; } = string.Empty;
    public int BatteryLevel { get; set; }
    public string NetworkType { get; set; } = string.Empty;
    public bool IsOnline { get; set; }
    public DateTime? LastHeartbeat { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TelecomJob> Jobs { get; set; } = new List<TelecomJob>();
}
