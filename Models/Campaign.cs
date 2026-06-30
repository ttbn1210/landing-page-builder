namespace TrackQR.Web.Models;

public class Campaign : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CampaignStatus Status { get; set; } = CampaignStatus.Draft;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }

    public ICollection<Placement> Placements { get; set; } = new List<Placement>();
    public ICollection<QRCode> QRCodes { get; set; } = new List<QRCode>();
}
