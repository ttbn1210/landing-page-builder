using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Features.Placements;

public class CreatePlacementHandler
{
    private readonly AppDbContext _db;

    public CreatePlacementHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Placement> HandleAsync(Guid campaignId, string name, string? locationNote, decimal? cost)
    {
        var placement = new Placement
        {
            CampaignId = campaignId,
            Name = name,
            LocationNote = locationNote,
            Cost = cost
        };

        _db.Placements.Add(placement);
        await _db.SaveChangesAsync();

        _db.AuditLogs.Add(new AuditLog
        {
            Action = "Placement.Created",
            EntityName = nameof(Placement),
            EntityId = placement.Id,
            JsonData = System.Text.Json.JsonSerializer.Serialize(new { placement.Name, placement.CampaignId })
        });
        await _db.SaveChangesAsync();

        return placement;
    }
}
