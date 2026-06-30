using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;
using TrackQR.Web.ViewModels;

namespace TrackQR.Web.Services.Analytics;

public class FunnelService
{
    private readonly AppDbContext _db;

    public FunnelService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<FunnelViewModel> GetFunnelAsync(Guid? campaignId = null, DateTime? from = null, DateTime? to = null)
    {
        var startDate = from ?? DateTime.UtcNow.AddDays(-30);
        var endDate = to ?? DateTime.UtcNow;

        var sessionsQuery = _db.VisitorSessions
            .Where(v => v.CreatedAt >= startDate && v.CreatedAt <= endDate);

        if (campaignId.HasValue)
            sessionsQuery = sessionsQuery.Where(v => v.QRCode.CampaignId == campaignId.Value);

        var totalScans = await sessionsQuery.CountAsync();
        var pageViews = await _db.TrackingEvents
            .Where(t => t.EventType == EventType.PageView && t.OccurredAt >= startDate && t.OccurredAt <= endDate)
            .CountAsync();
        var formOpens = await _db.TrackingEvents
            .Where(t => t.EventType == EventType.FormOpen && t.OccurredAt >= startDate && t.OccurredAt <= endDate)
            .CountAsync();
        var formSubmits = await _db.TrackingEvents
            .Where(t => t.EventType == EventType.FormSubmit && t.OccurredAt >= startDate && t.OccurredAt <= endDate)
            .CountAsync();
        var leads = await _db.Leads
            .Where(l => l.CreatedAt >= startDate && l.CreatedAt <= endDate)
            .CountAsync();

        return new FunnelViewModel
        {
            TotalScans = totalScans,
            PageViews = pageViews,
            FormOpens = formOpens,
            FormSubmits = formSubmits,
            LeadsCaptured = leads
        };
    }
}
