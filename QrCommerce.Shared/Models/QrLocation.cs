namespace QrCommerce.Shared.Models;

public class QrLocation
{
    public int Id { get; set; }
    public int CampaignId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string QrCode { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; } = string.Empty;
    public string VehicleCode { get; set; } = string.Empty;
    public int TotalScans { get; set; }
    public int TotalOrders { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Campaign Campaign { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<QrScanLog> ScanLogs { get; set; } = new List<QrScanLog>();
}
