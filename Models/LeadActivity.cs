namespace TrackQR.Web.Models;

public class LeadActivity : BaseEntity
{
    public Guid LeadId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public Lead Lead { get; set; } = null!;
}
