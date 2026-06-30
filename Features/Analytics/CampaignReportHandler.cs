using TrackQR.Web.Services.Analytics;
using TrackQR.Web.ViewModels;

namespace TrackQR.Web.Features.Analytics;

public class CampaignReportHandler
{
    private readonly AnalyticsService _analyticsService;

    public CampaignReportHandler(AnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public async Task<CampaignReportViewModel> HandleAsync(Guid campaignId, DateTime? from = null, DateTime? to = null)
    {
        return await _analyticsService.GetCampaignReportAsync(campaignId, from, to);
    }
}
