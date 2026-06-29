using Microsoft.AspNetCore.Mvc.RazorPages;
using QrCommerce.Web.Services;

namespace QrCommerce.Web.Pages.Admin;

public class DashboardModel : PageModel
{
    private readonly AnalyticsService _analytics;

    public DashboardModel(AnalyticsService analytics)
    {
        _analytics = analytics;
    }

    public DashboardStats Stats { get; set; } = new();
    public List<ChartDataPoint> DailyOrders { get; set; } = new();
    public List<ChartDataPoint> RevenueTrend { get; set; } = new();

    public async Task OnGetAsync()
    {
        Stats = await _analytics.GetDashboardStatsAsync();
        DailyOrders = await _analytics.GetDailyOrdersAsync();
        RevenueTrend = await _analytics.GetRevenueTrendAsync();
    }
}
