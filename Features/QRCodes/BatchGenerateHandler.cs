using TrackQR.Web.Services.QR;

namespace TrackQR.Web.Features.QRCodes;

public class BatchGenerateHandler
{
    private readonly QRCodeGeneratorService _generatorService;

    public BatchGenerateHandler(QRCodeGeneratorService generatorService)
    {
        _generatorService = generatorService;
    }

    public async Task<List<Models.QRCode>> HandleAsync(Guid campaignId, Guid landingPageId, List<Guid> placementIds)
    {
        return await _generatorService.BatchGenerateAsync(campaignId, landingPageId, placementIds);
    }
}
