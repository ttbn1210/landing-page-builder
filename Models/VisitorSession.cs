namespace TrackQR.Web.Models;

public class VisitorSession : BaseEntity
{
    public Guid QRCodeId { get; set; }
    public string SessionKey { get; set; } = string.Empty;
    public string? DeviceFingerprint { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DeviceType DeviceType { get; set; } = DeviceType.Unknown;
    public VisitorSource VisitorSource { get; set; } = VisitorSource.QRScan;
    public bool IsUniqueVisitor { get; set; } = true;
    public bool IsConverted { get; set; }
    public DateTime? LastSeenAt { get; set; }

    public QRCode QRCode { get; set; } = null!;
    public ICollection<TrackingEvent> TrackingEvents { get; set; } = new List<TrackingEvent>();
    public Lead? Lead { get; set; }
}
