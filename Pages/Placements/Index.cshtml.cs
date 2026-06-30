using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Pages.Placements;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<Placement> Placements { get; set; } = new();

    public async Task OnGetAsync()
    {
        Placements = await _db.Placements
            .Include(p => p.Campaign)
            .Include(p => p.QRCodes)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}
