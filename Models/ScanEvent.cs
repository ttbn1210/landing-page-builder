namespace TrackQR.Web.Models;

public class ScanEvent : BaseEntity
{
    public Guid QRCodeId { get; set; }
    public Guid SessionId { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? City { get; set; }
    public string? Referrer { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    public QRCode QRCode { get; set; } = null!;
    public VisitorSession Session { get; set; } = null!;
}
