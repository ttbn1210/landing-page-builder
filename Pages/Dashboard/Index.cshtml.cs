using Microsoft.AspNetCore.Mvc.RazorPages;
using TrackQR.Web.Services.Analytics;
using TrackQR.Web.ViewModels;

namespace TrackQR.Web.Pages.Dashboard;

public class IndexModel : PageModel
{
    private readonly AnalyticsService _analyticsService;

    public IndexModel(AnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public DashboardViewModel Data { get; set; } = new();

    public async Task OnGetAsync()
    {
        Data = await _analyticsService.GetDashboardDataAsync();
    }
}
