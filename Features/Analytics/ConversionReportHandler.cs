using TrackQR.Web.Services.Analytics;
using TrackQR.Web.ViewModels;

namespace TrackQR.Web.Features.Analytics;

public class ConversionReportHandler
{
    private readonly FunnelService _funnelService;

    public ConversionReportHandler(FunnelService funnelService)
    {
        _funnelService = funnelService;
    }

    public async Task<FunnelViewModel> HandleAsync(Guid? campaignId = null, DateTime? from = null, DateTime? to = null)
    {
        return await _funnelService.GetFunnelAsync(campaignId, from, to);
    }
}
