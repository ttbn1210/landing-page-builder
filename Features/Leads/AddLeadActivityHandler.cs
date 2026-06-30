using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Features.Leads;

public class AddLeadActivityHandler
{
    private readonly AppDbContext _db;

    public AddLeadActivityHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<LeadActivity> HandleAsync(Guid leadId, string action, string? notes)
    {
        var activity = new LeadActivity
        {
            LeadId = leadId,
            Action = action,
            Notes = notes
        };

        _db.LeadActivities.Add(activity);
        await _db.SaveChangesAsync();

        return activity;
    }
}
