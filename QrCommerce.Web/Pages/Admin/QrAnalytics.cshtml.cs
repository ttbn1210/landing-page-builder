using Microsoft.AspNetCore.Mvc.RazorPages;
using QrCommerce.Web.Services;

namespace QrCommerce.Web.Pages.Admin;

public class QrAnalyticsModel : PageModel
{
    private readonly AnalyticsService _analytics;

    public QrAnalyticsModel(AnalyticsService analytics) { _analytics = analytics; }

    public List<ChartDataPoint> TopLocations { get; set; } = new();
    public List<ChartDataPoint> SmsRate { get; set; } = new();
    public List<ChartDataPoint> Conversion { get; set; } = new();
    public List<ChartDataPoint> DailyOrders { get; set; } = new();

    public async Task OnGetAsync()
    {
        TopLocations = await _analytics.GetTopQrLocationsAsync();
        SmsRate = await _analytics.GetSmsSuccessRateAsync();
        Conversion = await _analytics.GetCampaignConversionAsync();
        DailyOrders = await _analytics.GetDailyOrdersAsync();
    }
}
