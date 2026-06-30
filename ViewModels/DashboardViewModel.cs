namespace TrackQR.Web.ViewModels;

public class DashboardViewModel
{
    public int TotalScansToday { get; set; }
    public int UniqueVisitorsToday { get; set; }
    public int LeadsToday { get; set; }
    public double ConversionRate { get; set; }
    public List<TopQRCodeViewModel> TopQRCodes { get; set; } = new();
    public List<TopCampaignViewModel> TopCampaigns { get; set; } = new();
}

public class TopQRCodeViewModel
{
    public string ShortCode { get; set; } = string.Empty;
    public int TotalScans { get; set; }
    public string CampaignName { get; set; } = string.Empty;
}

public class TopCampaignViewModel
{
    public string Name { get; set; } = string.Empty;
    public int TotalScans { get; set; }
    public int TotalLeads { get; set; }
}
