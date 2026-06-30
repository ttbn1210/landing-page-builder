using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Features.Placements;

namespace TrackQR.Web.Pages.Placements;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly CreatePlacementHandler _handler;

    public CreateModel(AppDbContext db, CreatePlacementHandler handler)
    {
        _db = db;
        _handler = handler;
    }

    [BindProperty]
    public Guid CampaignId { get; set; }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string? LocationNote { get; set; }

    [BindProperty]
    public decimal? Cost { get; set; }

    public List<SelectListItem> CampaignOptions { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadCampaigns();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadCampaigns();
            return Page();
        }

        await _handler.HandleAsync(CampaignId, Name, LocationNote, Cost);
        return RedirectToPage("Index");
    }

    private async Task LoadCampaigns()
    {
        CampaignOptions = await _db.Campaigns
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToListAsync();
    }
}
