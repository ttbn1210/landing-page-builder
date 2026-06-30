namespace TrackQR.Web.Models;

public class AuditLog : BaseEntity
{
    public string Action { get; set; } = string.Empty;
    public string? EntityName { get; set; }
    public Guid? EntityId { get; set; }
    public string? JsonData { get; set; }
}
