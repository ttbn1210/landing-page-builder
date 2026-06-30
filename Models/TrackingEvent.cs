namespace TrackQR.Web.Models;

public class TrackingEvent : BaseEntity
{
    public Guid SessionId { get; set; }
    public EventType EventType { get; set; }
    public string? ElementName { get; set; }
    public string? Value { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    public VisitorSession Session { get; set; } = null!;
}
