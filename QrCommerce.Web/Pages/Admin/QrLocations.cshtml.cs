using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Services;

namespace QrCommerce.Web.Pages.Admin;

public class QrLocationsModel : PageModel
{
    private readonly QrLocationService _service;

    public QrLocationsModel(QrLocationService service) { _service = service; }

    public List<QrLocation> Locations { get; set; } = new();
    [BindProperty(SupportsGet = true)] public new int Page { get; set; } = 1;
    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    public int TotalPages { get; set; }

    public async Task OnGetAsync()
    {
        var total = await _service.GetTotalCountAsync(Search);
        TotalPages = (int)Math.Ceiling(total / 20.0);
        Locations = await _service.GetLocationsAsync(Page, 20, Search);
    }
}
