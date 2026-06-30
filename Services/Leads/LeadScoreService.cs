using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Services.Leads;

public class LeadScoreService
{
    private readonly AppDbContext _db;

    public LeadScoreService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<int> CalculateScoreAsync(Guid sessionId)
    {
        var score = 0;

        var events = await _db.TrackingEvents
            .Where(t => t.SessionId == sessionId)
            .ToListAsync();

        // Base score for visiting
        score += 10;

        // Score for engagement
        foreach (var ev in events)
        {
            score += ev.EventType switch
            {
                EventType.PageView => 5,
                EventType.ButtonClick => 10,
                EventType.FormOpen => 15,
                EventType.FormSubmit => 30,
                EventType.CallClick => 25,
                EventType.Scroll50 => 5,
                EventType.Scroll100 => 10,
                _ => 0
            };
        }

        return Math.Min(score, 100);
    }

    public async Task UpdateLeadScoreAsync(Guid leadId)
    {
        var lead = await _db.Leads.FindAsync(leadId);
        if (lead == null) return;

        lead.Score = await CalculateScoreAsync(lead.SessionId);
        await _db.SaveChangesAsync();
    }
}
