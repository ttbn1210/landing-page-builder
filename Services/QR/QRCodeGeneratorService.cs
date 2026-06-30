using TrackQR.Web.Data;
using TrackQR.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace TrackQR.Web.Services.QR;

public class QRCodeGeneratorService
{
    private readonly AppDbContext _db;
    private readonly QRImageService _imageService;

    public QRCodeGeneratorService(AppDbContext db, QRImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<Models.QRCode> GenerateAsync(Guid campaignId, Guid placementId, Guid landingPageId, string? customShortCode = null)
    {
        var shortCode = customShortCode ?? GenerateShortCode();

        var existing = await _db.QRCodes.AnyAsync(q => q.ShortCode == shortCode);
        if (existing)
            throw new InvalidOperationException($"ShortCode '{shortCode}' already exists.");

        var qrCode = new Models.QRCode
        {
            CampaignId = campaignId,
            PlacementId = placementId,
            LandingPageId = landingPageId,
            ShortCode = shortCode,
            Status = QRCodeStatus.Active,
            IsActive = true
        };

        _db.QRCodes.Add(qrCode);
        await _db.SaveChangesAsync();

        _imageService.GenerateImage(qrCode.ShortCode);

        return qrCode;
    }

    public async Task<List<Models.QRCode>> BatchGenerateAsync(Guid campaignId, Guid landingPageId, List<Guid> placementIds)
    {
        var results = new List<Models.QRCode>();

        foreach (var placementId in placementIds)
        {
            var qrCode = await GenerateAsync(campaignId, placementId, landingPageId);
            results.Add(qrCode);
        }

        return results;
    }

    private static string GenerateShortCode()
    {
        var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var random = Random.Shared;
        var code = new char[8];
        for (int i = 0; i < code.Length; i++)
            code[i] = chars[random.Next(chars.Length)];
        return new string(code);
    }
}
