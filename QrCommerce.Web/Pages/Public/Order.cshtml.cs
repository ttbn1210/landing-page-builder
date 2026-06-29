using Microsoft.AspNetCore.Mvc.RazorPages;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Data;
using QrCommerce.Web.Services;

namespace QrCommerce.Web.Pages.Public;

public class OrderModel : PageModel
{
    private readonly QrLocationService _locationService;
    private readonly ProductService _productService;
    private readonly AppDbContext _db;

    public OrderModel(QrLocationService locationService, ProductService productService, AppDbContext db)
    {
        _locationService = locationService;
        _productService = productService;
        _db = db;
    }

    public QrLocation? QrLocation { get; set; }
    public List<Product> Products { get; set; } = new();
    public string QrCode { get; set; } = string.Empty;

    public async Task OnGetAsync(string qrCode)
    {
        QrCode = qrCode;
        QrLocation = await _locationService.GetByQrCodeAsync(qrCode);

        if (QrLocation != null)
        {
            QrLocation.TotalScans++;
            await _db.SaveChangesAsync();

            var scanLog = new QrScanLog
            {
                QrLocationId = QrLocation.Id,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                UserAgent = Request.Headers.UserAgent.ToString()
            };
            _db.QrScanLogs.Add(scanLog);
            await _db.SaveChangesAsync();

            Products = await _productService.GetActiveProductsAsync();
        }
    }
}
