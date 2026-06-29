using Microsoft.EntityFrameworkCore;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Data;

namespace QrCommerce.Web.Services;

public class QrLocationService
{
    private readonly AppDbContext _db;

    public QrLocationService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<QrLocation>> GetLocationsAsync(int page = 1, int pageSize = 20, string? search = null)
    {
        var query = _db.QrLocations.Include(q => q.Campaign).AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(q => q.Name.Contains(search) || q.QrCode.Contains(search) || q.Address.Contains(search));
        }
        return await query
            .OrderByDescending(q => q.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? search = null)
    {
        var query = _db.QrLocations.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(q => q.Name.Contains(search) || q.QrCode.Contains(search));
        }
        return await query.CountAsync();
    }

    public async Task<QrLocation?> GetByQrCodeAsync(string qrCode)
    {
        return await _db.QrLocations
            .Include(q => q.Campaign)
            .FirstOrDefaultAsync(q => q.QrCode == qrCode);
    }

    public async Task CreateAsync(QrLocation location)
    {
        _db.QrLocations.Add(location);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(QrLocation location)
    {
        _db.QrLocations.Update(location);
        await _db.SaveChangesAsync();
    }
}
