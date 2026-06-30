using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Infrastructure.Repositories;

public class QRCodeRepository
{
    private readonly AppDbContext _db;

    public QRCodeRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<QRCode>> GetAllAsync()
    {
        return await _db.QRCodes
            .Include(q => q.Campaign)
            .Include(q => q.Placement)
            .Include(q => q.LandingPage)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();
    }

    public async Task<QRCode?> GetByIdAsync(Guid id)
    {
        return await _db.QRCodes
            .Include(q => q.Campaign)
            .Include(q => q.Placement)
            .Include(q => q.LandingPage)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<QRCode?> GetByShortCodeAsync(string shortCode)
    {
        return await _db.QRCodes
            .Include(q => q.LandingPage)
            .FirstOrDefaultAsync(q => q.ShortCode == shortCode && q.IsActive);
    }

    public async Task UpdateAsync(QRCode qrCode)
    {
        _db.QRCodes.Update(qrCode);
        await _db.SaveChangesAsync();
    }
}
