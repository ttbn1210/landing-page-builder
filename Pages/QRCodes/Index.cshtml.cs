using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Pages.QRCodes;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<QRCode> QRCodes { get; set; } = new();

    public async Task OnGetAsync()
    {
        QRCodes = await _db.QRCodes
            .Include(q => q.Campaign)
            .Include(q => q.Placement)
            .Include(q => q.LandingPage)
            .Include(q => q.VisitorSessions)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();
    }
}
