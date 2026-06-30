using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.ViewModels;

namespace TrackQR.Web.Features.Analytics;

public class QRPerformanceHandler
{
    private readonly AppDbContext _db;

    public QRPerformanceHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<QRPerformanceViewModel>> HandleAsync(Guid? campaignId = null)
    {
        var query = _db.QRCodes
            .Include(q => q.ScanEvents)
            .Include(q => q.VisitorSessions)
            .Include(q => q.Campaign)
            .Include(q => q.Placement)
            .AsQueryable();

        if (campaignId.HasValue)
            query = query.Where(q => q.CampaignId == campaignId.Value);

        return await query.Select(q => new QRPerformanceViewModel
        {
            ShortCode = q.ShortCode,
            CampaignName = q.Campaign.Name,
            PlacementName = q.Placement.Name,
            TotalScans = q.ScanEvents.Count,
            UniqueVisitors = q.VisitorSessions.Count(v => v.IsUniqueVisitor),
            Conversions = q.VisitorSessions.Count(v => v.IsConverted),
            ConversionRate = q.VisitorSessions.Count > 0
                ? (double)q.VisitorSessions.Count(v => v.IsConverted) / q.VisitorSessions.Count * 100
                : 0
        })
        .OrderByDescending(q => q.TotalScans)
        .ToListAsync();
    }
}
