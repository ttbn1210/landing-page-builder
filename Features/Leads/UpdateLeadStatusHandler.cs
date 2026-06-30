using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Features.Leads;

public class UpdateLeadStatusHandler
{
    private readonly AppDbContext _db;

    public UpdateLeadStatusHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> HandleAsync(Guid leadId, LeadStatus newStatus, string? notes = null)
    {
        var lead = await _db.Leads.FindAsync(leadId);
        if (lead == null) return false;

        var oldStatus = lead.Status;
        lead.Status = newStatus;

        var activity = new LeadActivity
        {
            LeadId = leadId,
            Action = $"Status changed: {oldStatus} → {newStatus}",
            Notes = notes
        };

        _db.LeadActivities.Add(activity);
        await _db.SaveChangesAsync();

        return true;
    }
}
