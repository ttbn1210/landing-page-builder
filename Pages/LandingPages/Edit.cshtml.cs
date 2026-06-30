using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrackQR.Web.Data;
using TrackQR.Web.Features.LandingPages;
using TrackQR.Web.Models;

namespace TrackQR.Web.Pages.LandingPages;

public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly PublishLandingPageHandler _publishHandler;

    public EditModel(AppDbContext db, PublishLandingPageHandler publishHandler)
    {
        _db = db;
        _publishHandler = publishHandler;
    }

    [BindProperty]
    public Guid Id { get; set; }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public LandingPageStatus Status { get; set; }

    [BindProperty]
    public string? HtmlContent { get; set; }

    [BindProperty]
    public string? CssContent { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var page = await _db.LandingPages.FindAsync(id);
        if (page == null) return NotFound();

        Id = page.Id;
        Name = page.Name;
        Status = page.Status;
        HtmlContent = page.HtmlContent;
        CssContent = page.CssContent;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var page = await _db.LandingPages.FindAsync(Id);
        if (page == null) return NotFound();

        page.Name = Name;
        page.Status = Status;
        page.HtmlContent = HtmlContent;
        page.CssContent = CssContent;

        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostPublishAsync()
    {
        await _publishHandler.HandleAsync(Id);
        return RedirectToPage("Edit", new { id = Id });
    }
}
