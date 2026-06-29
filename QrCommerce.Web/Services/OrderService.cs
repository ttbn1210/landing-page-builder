using Microsoft.EntityFrameworkCore;
using QrCommerce.Shared.DTOs;
using QrCommerce.Shared.Enums;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Data;

namespace QrCommerce.Web.Services;

public class OrderService
{
    private readonly AppDbContext _db;
    private readonly TelecomService _telecomService;

    public OrderService(AppDbContext db, TelecomService telecomService)
    {
        _db = db;
        _telecomService = telecomService;
    }

    public async Task<Order> CreateOrderAsync(CheckoutDto dto)
    {
        var qrLocation = await _db.QrLocations.FirstOrDefaultAsync(q => q.QrCode == dto.QrCode);
        if (qrLocation == null)
            throw new InvalidOperationException("Invalid QR code");

        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Phone == dto.Phone);
        if (customer == null)
        {
            customer = new Customer
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                FirstVisitAt = DateTime.UtcNow,
                FirstOrderAt = DateTime.UtcNow
            };
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
        }

        var orderCode = $"ORD{DateTime.UtcNow:yyyyMMddHHmmss}{customer.Id:D4}";
        var order = new Order
        {
            OrderCode = orderCode,
            CustomerId = customer.Id,
            QrLocationId = qrLocation.Id,
            Status = OrderStatus.Pending
        };

        decimal total = 0;
        foreach (var item in dto.Items)
        {
            var product = await _db.Products.FindAsync(item.ProductId);
            if (product == null || !product.IsActive) continue;

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                TotalPrice = product.Price * item.Quantity
            };
            order.Items.Add(orderItem);
            total += orderItem.TotalPrice;
        }

        order.TotalAmount = total;
        _db.Orders.Add(order);

        customer.TotalOrders++;
        customer.TotalSpent += total;
        customer.LastOrderAt = DateTime.UtcNow;

        qrLocation.TotalOrders++;

        await _db.SaveChangesAsync();

        var smsMessage = $"Thank you for your order.\n\nOrder code: {orderCode}\n\nYour order has been received.";
        await _telecomService.EnqueueAsync(customer.Phone, smsMessage);

        return order;
    }

    public async Task<List<Order>> GetOrdersAsync(int page = 1, int pageSize = 20, string? search = null)
    {
        var query = _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.QrLocation)
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(o => o.OrderCode.Contains(search) ||
                                     o.Customer.Name.Contains(search) ||
                                     o.Customer.Phone.Contains(search));
        }

        return await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? search = null)
    {
        var query = _db.Orders.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(o => o.OrderCode.Contains(search) ||
                                     o.Customer.Name.Contains(search) ||
                                     o.Customer.Phone.Contains(search));
        }
        return await query.CountAsync();
    }

    public async Task UpdateStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            await _db.SaveChangesAsync();
        }
    }

    public async Task CancelOrderAsync(int orderId)
    {
        await UpdateStatusAsync(orderId, OrderStatus.Cancelled);
    }
}
