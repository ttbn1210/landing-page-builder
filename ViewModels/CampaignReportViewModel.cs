namespace TrackQR.Web.ViewModels;

public class CampaignReportViewModel
{
    public string CampaignName { get; set; } = string.Empty;
    public int TotalScans { get; set; }
    public int TotalPlacements { get; set; }
    public int TotalQRCodes { get; set; }
    public List<DailyScanViewModel> DailyScans { get; set; } = new();
}

public class DailyScanViewModel
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
}
