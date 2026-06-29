using Microsoft.EntityFrameworkCore;
using QrCommerce.Shared.Enums;
using QrCommerce.Web.Data;

namespace QrCommerce.Web.Services;

public class AnalyticsService
{
    private readonly AppDbContext _db;

    public AnalyticsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        return new DashboardStats
        {
            TotalCampaigns = await _db.Campaigns.CountAsync(),
            TotalQrScans = await _db.QrScanLogs.CountAsync(),
            TotalOrders = await _db.Orders.CountAsync(),
            Revenue = await _db.Orders.Where(o => o.Status != OrderStatus.Cancelled).SumAsync(o => o.TotalAmount),
            RepeatCustomers = await _db.Customers.CountAsync(c => c.TotalOrders > 1),
            SmsSent = await _db.TelecomJobs.CountAsync(j => j.Status == TelecomJobStatus.Sent),
            FailedSms = await _db.TelecomJobs.CountAsync(j => j.Status == TelecomJobStatus.Failed),
            OnlineDevices = await _db.TelecomDevices.CountAsync(d => d.IsOnline),
            OfflineDevices = await _db.TelecomDevices.CountAsync(d => !d.IsOnline)
        };
    }

    public async Task<List<ChartDataPoint>> GetDailyOrdersAsync(int days = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);
        var orders = await _db.Orders
            .Where(o => o.CreatedAt >= startDate)
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new ChartDataPoint { Label = g.Key.ToString("MM/dd"), Value = g.Count() })
            .OrderBy(x => x.Label)
            .ToListAsync();
        return orders;
    }

    public async Task<List<ChartDataPoint>> GetRevenueTrendAsync(int days = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);
        var revenue = await _db.Orders
            .Where(o => o.CreatedAt >= startDate && o.Status != OrderStatus.Cancelled)
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new ChartDataPoint { Label = g.Key.ToString("MM/dd"), Value = (double)g.Sum(o => o.TotalAmount) })
            .OrderBy(x => x.Label)
            .ToListAsync();
        return revenue;
    }

    public async Task<List<ChartDataPoint>> GetTopQrLocationsAsync(int count = 10)
    {
        return await _db.QrLocations
            .OrderByDescending(q => q.TotalScans)
            .Take(count)
            .Select(q => new ChartDataPoint { Label = q.Name, Value = q.TotalScans })
            .ToListAsync();
    }

    public async Task<List<ChartDataPoint>> GetSmsSuccessRateAsync()
    {
        var sent = await _db.TelecomJobs.CountAsync(j => j.Status == TelecomJobStatus.Sent);
        var failed = await _db.TelecomJobs.CountAsync(j => j.Status == TelecomJobStatus.Failed);
        var pending = await _db.TelecomJobs.CountAsync(j => j.Status == TelecomJobStatus.Pending);
        return new List<ChartDataPoint>
        {
            new() { Label = "Sent", Value = sent },
            new() { Label = "Failed", Value = failed },
            new() { Label = "Pending", Value = pending }
        };
    }

    public async Task<List<ChartDataPoint>> GetCampaignConversionAsync()
    {
        var campaigns = await _db.Campaigns
            .Include(c => c.QrLocations)
            .ToListAsync();

        var results = new List<ChartDataPoint>();
        foreach (var campaign in campaigns.Take(10))
        {
            var scans = campaign.QrLocations.Sum(q => q.TotalScans);
            var orders = campaign.QrLocations.Sum(q => q.TotalOrders);
            var rate = scans > 0 ? (double)orders / scans * 100 : 0;
            results.Add(new ChartDataPoint { Label = campaign.Name, Value = Math.Round(rate, 1) });
        }
        return results;
    }
}

public class DashboardStats
{
    public int TotalCampaigns { get; set; }
    public int TotalQrScans { get; set; }
    public int TotalOrders { get; set; }
    public decimal Revenue { get; set; }
    public int RepeatCustomers { get; set; }
    public int SmsSent { get; set; }
    public int FailedSms { get; set; }
    public int OnlineDevices { get; set; }
    public int OfflineDevices { get; set; }
}

public class ChartDataPoint
{
    public string Label { get; set; } = string.Empty;
    public double Value { get; set; }
}
