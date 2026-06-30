using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrackQR.Web.Features.Campaigns;

namespace TrackQR.Web.Pages.Campaigns;

public class CreateModel : PageModel
{
    private readonly CreateCampaignHandler _handler;

    public CreateModel(CreateCampaignHandler handler)
    {
        _handler = handler;
    }

    [BindProperty]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public string? Description { get; set; }

    [BindProperty]
    public DateTime? StartDate { get; set; }

    [BindProperty]
    public DateTime? EndDate { get; set; }

    [BindProperty]
    public decimal? Budget { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        await _handler.HandleAsync(Name, Description, StartDate, EndDate, Budget);
        return RedirectToPage("Index");
    }
}
