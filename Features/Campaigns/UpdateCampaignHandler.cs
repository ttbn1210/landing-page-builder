using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Features.Campaigns;

public class UpdateCampaignHandler
{
    private readonly AppDbContext _db;

    public UpdateCampaignHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> HandleAsync(Guid id, string name, string? description, CampaignStatus status, DateTime? startDate, DateTime? endDate, decimal? budget)
    {
        var campaign = await _db.Campaigns.FindAsync(id);
        if (campaign == null) return false;

        campaign.Name = name;
        campaign.Description = description;
        campaign.Status = status;
        campaign.StartDate = startDate;
        campaign.EndDate = endDate;
        campaign.Budget = budget;

        await _db.SaveChangesAsync();

        _db.AuditLogs.Add(new AuditLog
        {
            Action = "Campaign.Updated",
            EntityName = nameof(Campaign),
            EntityId = campaign.Id,
            JsonData = System.Text.Json.JsonSerializer.Serialize(new { campaign.Name, campaign.Status })
        });
        await _db.SaveChangesAsync();

        return true;
    }
}
