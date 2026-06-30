namespace TrackQR.Web.Models;

public class Lead : BaseEntity
{
    public Guid SessionId { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Message { get; set; }
    public LeadStatus Status { get; set; } = LeadStatus.New;
    public int Score { get; set; }

    public VisitorSession Session { get; set; } = null!;
    public ICollection<LeadActivity> Activities { get; set; } = new List<LeadActivity>();
}
