using TrackQR.Web.Services.QR;

namespace TrackQR.Web.Features.QRCodes;

public class GenerateQRCodeHandler
{
    private readonly QRCodeGeneratorService _generatorService;

    public GenerateQRCodeHandler(QRCodeGeneratorService generatorService)
    {
        _generatorService = generatorService;
    }

    public async Task<Models.QRCode> HandleAsync(Guid campaignId, Guid placementId, Guid landingPageId, string? customShortCode = null)
    {
        return await _generatorService.GenerateAsync(campaignId, placementId, landingPageId, customShortCode);
    }
}
