using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Features.Campaigns;

public class CreateCampaignHandler
{
    private readonly AppDbContext _db;

    public CreateCampaignHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Campaign> HandleAsync(string name, string? description, DateTime? startDate, DateTime? endDate, decimal? budget)
    {
        var campaign = new Campaign
        {
            Name = name,
            Description = description,
            Status = CampaignStatus.Draft,
            StartDate = startDate,
            EndDate = endDate,
            Budget = budget
        };

        _db.Campaigns.Add(campaign);
        await _db.SaveChangesAsync();

        _db.AuditLogs.Add(new AuditLog
        {
            Action = "Campaign.Created",
            EntityName = nameof(Campaign),
            EntityId = campaign.Id,
            JsonData = System.Text.Json.JsonSerializer.Serialize(new { campaign.Name, campaign.Status })
        });
        await _db.SaveChangesAsync();

        return campaign;
    }
}
