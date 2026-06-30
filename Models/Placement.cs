namespace TrackQR.Web.Models;

public class Placement : BaseEntity
{
    public Guid CampaignId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LocationNote { get; set; }
    public decimal? Cost { get; set; }

    public Campaign Campaign { get; set; } = null!;
    public ICollection<QRCode> QRCodes { get; set; } = new List<QRCode>();
}
