namespace TrackQR.Web.ViewModels;

public class QRPerformanceViewModel
{
    public string ShortCode { get; set; } = string.Empty;
    public string CampaignName { get; set; } = string.Empty;
    public string PlacementName { get; set; } = string.Empty;
    public int TotalScans { get; set; }
    public int UniqueVisitors { get; set; }
    public int Conversions { get; set; }
    public double ConversionRate { get; set; }
}
