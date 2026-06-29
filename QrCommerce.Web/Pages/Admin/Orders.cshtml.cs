using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QrCommerce.Shared.Enums;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Services;

namespace QrCommerce.Web.Pages.Admin;

public class OrdersModel : PageModel
{
    private readonly OrderService _service;

    public OrdersModel(OrderService service) { _service = service; }

    public List<Order> Orders { get; set; } = new();
    [BindProperty(SupportsGet = true)] public new int Page { get; set; } = 1;
    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    public int TotalPages { get; set; }

    public async Task OnGetAsync()
    {
        var total = await _service.GetTotalCountAsync(Search);
        TotalPages = (int)Math.Ceiling(total / 20.0);
        Orders = await _service.GetOrdersAsync(Page, 20, Search);
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, string status)
    {
        if (Enum.TryParse<OrderStatus>(status, out var orderStatus))
        {
            await _service.UpdateStatusAsync(orderId, orderStatus);
        }
        return RedirectToPage();
    }
}
