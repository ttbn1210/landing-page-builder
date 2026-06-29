namespace QrCommerce.Shared.Models;

public class QrScanLog
{
    public int Id { get; set; }
    public int QrLocationId { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public QrLocation QrLocation { get; set; } = null!;
}
