using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;
using TrackQR.Web.ViewModels;

namespace TrackQR.Web.Services.Analytics;

public class AnalyticsService
{
    private readonly AppDbContext _db;

    public AnalyticsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardViewModel> GetDashboardDataAsync()
    {
        var today = DateTime.UtcNow.Date;

        var totalScansToday = await _db.ScanEvents
            .CountAsync(s => s.OccurredAt >= today);

        var uniqueVisitorsToday = await _db.VisitorSessions
            .CountAsync(v => v.CreatedAt >= today && v.IsUniqueVisitor);

        var leadsToday = await _db.Leads
            .CountAsync(l => l.CreatedAt >= today);

        var totalSessions = await _db.VisitorSessions.CountAsync(v => v.CreatedAt >= today);
        var convertedSessions = await _db.VisitorSessions.CountAsync(v => v.CreatedAt >= today && v.IsConverted);
        var conversionRate = totalSessions > 0 ? (double)convertedSessions / totalSessions * 100 : 0;

        var topQRCodes = await _db.QRCodes
            .Include(q => q.ScanEvents)
            .OrderByDescending(q => q.ScanEvents.Count)
            .Take(5)
            .Select(q => new TopQRCodeViewModel
            {
                ShortCode = q.ShortCode,
                TotalScans = q.ScanEvents.Count,
                CampaignName = q.Campaign.Name
            })
            .ToListAsync();

        var topCampaigns = await _db.Campaigns
            .Where(c => c.Status == CampaignStatus.Active)
            .Select(c => new TopCampaignViewModel
            {
                Name = c.Name,
                TotalScans = c.QRCodes.SelectMany(q => q.ScanEvents).Count(),
                TotalLeads = c.QRCodes
                    .SelectMany(q => q.VisitorSessions)
                    .Count(vs => vs.IsConverted)
            })
            .OrderByDescending(c => c.TotalScans)
            .Take(5)
            .ToListAsync();

        return new DashboardViewModel
        {
            TotalScansToday = totalScansToday,
            UniqueVisitorsToday = uniqueVisitorsToday,
            LeadsToday = leadsToday,
            ConversionRate = Math.Round(conversionRate, 1),
            TopQRCodes = topQRCodes,
            TopCampaigns = topCampaigns
        };
    }

    public async Task<CampaignReportViewModel> GetCampaignReportAsync(Guid campaignId, DateTime? from = null, DateTime? to = null)
    {
        var startDate = from ?? DateTime.UtcNow.AddDays(-30);
        var endDate = to ?? DateTime.UtcNow;

        var campaign = await _db.Campaigns
            .Include(c => c.Placements)
            .Include(c => c.QRCodes)
                .ThenInclude(q => q.ScanEvents)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign == null)
            return new CampaignReportViewModel();

        var scansByDay = await _db.ScanEvents
            .Where(s => s.QRCode.CampaignId == campaignId && s.OccurredAt >= startDate && s.OccurredAt <= endDate)
            .GroupBy(s => s.OccurredAt.Date)
            .Select(g => new DailyScanViewModel { Date = g.Key, Count = g.Count() })
            .OrderBy(d => d.Date)
            .ToListAsync();

        return new CampaignReportViewModel
        {
            CampaignName = campaign.Name,
            TotalScans = campaign.QRCodes.SelectMany(q => q.ScanEvents).Count(),
            TotalPlacements = campaign.Placements.Count,
            TotalQRCodes = campaign.QRCodes.Count,
            DailyScans = scansByDay
        };
    }
}
