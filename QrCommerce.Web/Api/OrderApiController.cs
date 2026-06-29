using Microsoft.AspNetCore.Mvc;
using QrCommerce.Shared.DTOs;
using QrCommerce.Web.Services;

namespace QrCommerce.Web.Api;

[ApiController]
[Route("api/orders")]
public class OrderApiController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderApiController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CheckoutDto dto)
    {
        if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Phone) || string.IsNullOrEmpty(dto.QrCode))
            return BadRequest(new { error = "Name, Phone, and QrCode are required" });

        if (dto.Items == null || dto.Items.Count == 0)
            return BadRequest(new { error = "At least one item required" });

        try
        {
            var order = await _orderService.CreateOrderAsync(dto);
            return Ok(new { success = true, orderCode = order.OrderCode, orderId = order.Id });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
