using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Services;

namespace QrCommerce.Web.Pages.Admin;

public class TelecomDevicesModel : PageModel
{
    private readonly DeviceService _service;

    public TelecomDevicesModel(DeviceService service) { _service = service; }

    public List<TelecomDevice> Devices { get; set; } = new();
    [BindProperty(SupportsGet = true)] public new int Page { get; set; } = 1;
    public int TotalPages { get; set; }

    public async Task OnGetAsync()
    {
        var total = await _service.GetDevicesCountAsync();
        TotalPages = (int)Math.Ceiling(total / 20.0);
        Devices = await _service.GetDevicesAsync(Page);
    }
}
