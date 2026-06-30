using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrackQR.Web.Data;
using TrackQR.Web.Features.Placements;

namespace TrackQR.Web.Pages.Placements;

public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly UpdatePlacementHandler _handler;

    public EditModel(AppDbContext db, UpdatePlacementHandler handler)
    {
        _db = db;
        _handler = handler;
    }

    [BindProperty]
    public Guid Id { get; set; }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string? LocationNote { get; set; }

    [BindProperty]
    public decimal? Cost { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var placement = await _db.Placements.FindAsync(id);
        if (placement == null) return NotFound();

        Id = placement.Id;
        Name = placement.Name;
        LocationNote = placement.LocationNote;
        Cost = placement.Cost;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        await _handler.HandleAsync(Id, Name, LocationNote, Cost);
        return RedirectToPage("Index");
    }
}
