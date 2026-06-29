using Microsoft.EntityFrameworkCore;
using QrCommerce.Shared.Enums;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Data;

namespace QrCommerce.Web.Services;

public class CampaignService
{
    private readonly AppDbContext _db;

    public CampaignService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Campaign>> GetCampaignsAsync(int page = 1, int pageSize = 20, string? search = null)
    {
        var query = _db.Campaigns.Include(c => c.QrLocations).AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => c.Name.Contains(search) || c.CampaignCode.Contains(search));
        }
        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? search = null)
    {
        var query = _db.Campaigns.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => c.Name.Contains(search) || c.CampaignCode.Contains(search));
        }
        return await query.CountAsync();
    }

    public async Task<Campaign?> GetByIdAsync(int id)
    {
        return await _db.Campaigns.Include(c => c.QrLocations).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateAsync(Campaign campaign)
    {
        _db.Campaigns.Add(campaign);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Campaign campaign)
    {
        _db.Campaigns.Update(campaign);
        await _db.SaveChangesAsync();
    }
}
