namespace TrackQR.Web.Models;

public class Lead : BaseEntity
{
    // Thông tin contact chuẩn hóa (nếu detect được)
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string? FormDataJson { get; set; }

    // Metadata form
    public string? FormName { get; set; } // enroll_form
    public string? FormType { get; set; } // contact, booking, survey, order
    public LeadStatus Status { get; set; } = LeadStatus.New;
    public int Score { get; set; }

    public Guid SessionId { get; set; }
    public VisitorSession Session { get; set; } = null!;
    public ICollection<LeadActivity> Activities { get; set; } = new List<LeadActivity>();
}
