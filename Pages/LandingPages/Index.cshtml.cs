using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Pages.LandingPages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<LandingPage> LandingPages { get; set; } = new();

    public async Task OnGetAsync()
    {
        LandingPages = await _db.LandingPages
            .Include(lp => lp.QRCodes)
            .OrderByDescending(lp => lp.CreatedAt)
            .ToListAsync();
    }
}
