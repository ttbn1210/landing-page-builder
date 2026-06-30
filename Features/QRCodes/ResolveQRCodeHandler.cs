using TrackQR.Web.Infrastructure.Cache;
using TrackQR.Web.Infrastructure.Repositories;

namespace TrackQR.Web.Features.QRCodes;

public class ResolveQRCodeHandler
{
    private readonly QRCodeRepository _repo;
    private readonly MemoryCacheService _cache;

    public ResolveQRCodeHandler(QRCodeRepository repo, MemoryCacheService cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<Models.QRCode?> HandleAsync(string shortCode)
    {
        var cached = _cache.Get<Models.QRCode>(CacheKeys.QRCode(shortCode));
        if (cached != null) return cached;

        var qrCode = await _repo.GetByShortCodeAsync(shortCode);
        if (qrCode != null)
            _cache.Set(CacheKeys.QRCode(shortCode), qrCode, TimeSpan.FromMinutes(10));

        return qrCode;
    }
}
