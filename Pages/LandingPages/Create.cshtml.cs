using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Pages.LandingPages;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;

    public CreateModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public TemplateType TemplateType { get; set; }

    [BindProperty]
    public string? HtmlContent { get; set; }

    [BindProperty]
    public string? CssContent { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var page = new LandingPage
        {
            Name = Name,
            TemplateType = TemplateType,
            HtmlContent = HtmlContent,
            CssContent = CssContent,
            Status = LandingPageStatus.Draft
        };

        _db.LandingPages.Add(page);
        await _db.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
