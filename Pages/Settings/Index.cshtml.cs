using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;

namespace TrackQR.Web.Pages.Settings;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public IndexModel(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public string BaseUrl => _config["App:BaseUrl"] ?? "https://localhost:5001";
    public int TotalCampaigns { get; set; }
    public int TotalQRCodes { get; set; }
    public int TotalLeads { get; set; }
    public int TotalLandingPages { get; set; }

    public async Task OnGetAsync()
    {
        TotalCampaigns = await _db.Campaigns.CountAsync();
        TotalQRCodes = await _db.QRCodes.CountAsync();
        TotalLeads = await _db.Leads.CountAsync();
        TotalLandingPages = await _db.LandingPages.CountAsync();
    }
}
