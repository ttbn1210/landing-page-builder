using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Features.Placements;

public class UpdatePlacementHandler
{
    private readonly AppDbContext _db;

    public UpdatePlacementHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> HandleAsync(Guid id, string name, string? locationNote, decimal? cost)
    {
        var placement = await _db.Placements.FindAsync(id);
        if (placement == null) return false;

        placement.Name = name;
        placement.LocationNote = locationNote;
        placement.Cost = cost;

        await _db.SaveChangesAsync();
        return true;
    }
}
