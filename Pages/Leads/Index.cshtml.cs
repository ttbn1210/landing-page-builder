using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Pages.Leads;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<Lead> Leads { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public LeadStatus? FilterStatus { get; set; }

    public async Task OnGetAsync()
    {
        var query = _db.Leads
            .Include(l => l.Session)
                .ThenInclude(s => s.QRCode)
                    .ThenInclude(q => q.Campaign)
            .AsQueryable();

        if (FilterStatus.HasValue)
            query = query.Where(l => l.Status == FilterStatus.Value);

        Leads = await query.OrderByDescending(l => l.CreatedAt).ToListAsync();
    }
}
