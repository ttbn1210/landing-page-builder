using QrCommerce.Shared.Enums;

namespace QrCommerce.Shared.Models;

public class TelecomLog
{
    public int Id { get; set; }
    public int TelecomJobId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public TelecomJobStatus Status { get; set; }
    public string RawResponse { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public TelecomJob TelecomJob { get; set; } = null!;
}
