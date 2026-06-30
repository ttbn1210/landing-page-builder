using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Features.LandingPages;

public class PublishLandingPageHandler
{
    private readonly AppDbContext _db;

    public PublishLandingPageHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> HandleAsync(Guid id)
    {
        var page = await _db.LandingPages.FindAsync(id);
        if (page == null) return false;

        page.Status = LandingPageStatus.Published;
        await _db.SaveChangesAsync();

        _db.AuditLogs.Add(new AuditLog
        {
            Action = "LandingPage.Published",
            EntityName = nameof(LandingPage),
            EntityId = page.Id
        });
        await _db.SaveChangesAsync();

        return true;
    }
}
