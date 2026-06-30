using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Features.Campaigns;

public class ArchiveCampaignHandler
{
    private readonly AppDbContext _db;

    public ArchiveCampaignHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> HandleAsync(Guid id)
    {
        var campaign = await _db.Campaigns.FindAsync(id);
        if (campaign == null) return false;

        campaign.Status = CampaignStatus.Archived;
        await _db.SaveChangesAsync();

        _db.AuditLogs.Add(new AuditLog
        {
            Action = "Campaign.Archived",
            EntityName = nameof(Campaign),
            EntityId = campaign.Id
        });
        await _db.SaveChangesAsync();

        return true;
    }
}
