namespace QrCommerce.Shared.DTOs;

public class JobReportDto
{
    public int JobId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string RawResponse { get; set; } = string.Empty;
}
