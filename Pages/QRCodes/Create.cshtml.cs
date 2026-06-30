using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Features.QRCodes;
using TrackQR.Web.Models;

namespace TrackQR.Web.Pages.QRCodes;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly GenerateQRCodeHandler _handler;

    public CreateModel(AppDbContext db, GenerateQRCodeHandler handler)
    {
        _db = db;
        _handler = handler;
    }

    [BindProperty]
    public Guid CampaignId { get; set; }

    [BindProperty]
    public Guid PlacementId { get; set; }

    [BindProperty]
    public Guid LandingPageId { get; set; }

    [BindProperty]
    public string? CustomShortCode { get; set; }

    public List<SelectListItem> CampaignOptions { get; set; } = new();
    public List<SelectListItem> PlacementOptions { get; set; } = new();
    public List<SelectListItem> LandingPageOptions { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadOptions();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadOptions();
            return Page();
        }

        try
        {
            var shortCode = string.IsNullOrWhiteSpace(CustomShortCode) ? null : CustomShortCode.Trim();
            await _handler.HandleAsync(CampaignId, PlacementId, LandingPageId, shortCode);
            return RedirectToPage("Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            await LoadOptions();
            return Page();
        }
    }

    private async Task LoadOptions()
    {
        CampaignOptions = await _db.Campaigns
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToListAsync();

        PlacementOptions = await _db.Placements
            .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
            .ToListAsync();

        LandingPageOptions = await _db.LandingPages
            .Where(lp => lp.Status == LandingPageStatus.Published)
            .Select(lp => new SelectListItem { Value = lp.Id.ToString(), Text = lp.Name })
            .ToListAsync();
    }
}
