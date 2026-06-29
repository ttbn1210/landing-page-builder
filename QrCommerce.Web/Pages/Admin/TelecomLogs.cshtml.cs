using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Services;

namespace QrCommerce.Web.Pages.Admin;

public class TelecomLogsModel : PageModel
{
    private readonly TelecomService _service;

    public TelecomLogsModel(TelecomService service) { _service = service; }

    public List<TelecomLog> Logs { get; set; } = new();
    [BindProperty(SupportsGet = true)] public new int Page { get; set; } = 1;
    public int TotalPages { get; set; }

    public async Task OnGetAsync()
    {
        var total = await _service.GetLogsCountAsync();
        TotalPages = (int)Math.Ceiling(total / 20.0);
        Logs = await _service.GetLogsAsync(Page);
    }
}
