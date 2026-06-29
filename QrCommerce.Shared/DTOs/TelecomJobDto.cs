namespace QrCommerce.Shared.DTOs;

public class TelecomJobDto
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int Priority { get; set; }
}
