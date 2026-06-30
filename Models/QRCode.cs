namespace TrackQR.Web.Models;

public class QRCode : BaseEntity
{
    public Guid CampaignId { get; set; }
    public Guid PlacementId { get; set; }
    public Guid LandingPageId { get; set; }
    public string ShortCode { get; set; } = string.Empty;
    public QRCodeStatus Status { get; set; } = QRCodeStatus.Active;
    public bool IsActive { get; set; } = true;

    public Campaign Campaign { get; set; } = null!;
    public Placement Placement { get; set; } = null!;
    public LandingPage LandingPage { get; set; } = null!;
    public ICollection<VisitorSession> VisitorSessions { get; set; } = new List<VisitorSession>();
    public ICollection<ScanEvent> ScanEvents { get; set; } = new List<ScanEvent>();
}
