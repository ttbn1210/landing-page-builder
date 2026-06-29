using QrCommerce.Shared.Enums;

namespace QrCommerce.Shared.Models;

public class TelecomJob
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int Priority { get; set; }
    public int RetryCount { get; set; }
    public TelecomJobStatus Status { get; set; } = TelecomJobStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }

    public TelecomDevice Device { get; set; } = null!;
    public ICollection<TelecomLog> Logs { get; set; } = new List<TelecomLog>();
}
