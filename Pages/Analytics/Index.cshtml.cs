using Microsoft.AspNetCore.Mvc.RazorPages;
using TrackQR.Web.Features.Analytics;
using TrackQR.Web.ViewModels;

namespace TrackQR.Web.Pages.Analytics;

public class IndexModel : PageModel
{
    private readonly QRPerformanceHandler _qrHandler;
    private readonly ConversionReportHandler _conversionHandler;

    public IndexModel(QRPerformanceHandler qrHandler, ConversionReportHandler conversionHandler)
    {
        _qrHandler = qrHandler;
        _conversionHandler = conversionHandler;
    }

    public FunnelViewModel Funnel { get; set; } = new();
    public List<QRPerformanceViewModel> QRPerformance { get; set; } = new();

    public async Task OnGetAsync()
    {
        Funnel = await _conversionHandler.HandleAsync();
        QRPerformance = await _qrHandler.HandleAsync();
    }
}
