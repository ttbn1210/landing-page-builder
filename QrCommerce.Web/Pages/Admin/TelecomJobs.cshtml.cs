using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Services;

namespace QrCommerce.Web.Pages.Admin;

public class TelecomJobsModel : PageModel
{
    private readonly TelecomService _service;

    public TelecomJobsModel(TelecomService service) { _service = service; }

    public List<TelecomJob> Jobs { get; set; } = new();
    [BindProperty(SupportsGet = true)] public new int Page { get; set; } = 1;
    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    public int TotalPages { get; set; }

    public async Task OnGetAsync()
    {
        var total = await _service.GetJobsCountAsync(Search);
        TotalPages = (int)Math.Ceiling(total / 20.0);
        Jobs = await _service.GetJobsAsync(Page, 20, Search);
    }
}
