using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Infrastructure.Repositories;

public class CampaignRepository
{
    private readonly AppDbContext _db;

    public CampaignRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Campaign>> GetAllAsync()
    {
        return await _db.Campaigns
            .Include(c => c.Placements)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Campaign?> GetByIdAsync(Guid id)
    {
        return await _db.Campaigns
            .Include(c => c.Placements)
            .Include(c => c.QRCodes)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Campaign> CreateAsync(Campaign campaign)
    {
        _db.Campaigns.Add(campaign);
        await _db.SaveChangesAsync();
        return campaign;
    }

    public async Task UpdateAsync(Campaign campaign)
    {
        _db.Campaigns.Update(campaign);
        await _db.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        var campaign = await _db.Campaigns.FindAsync(id);
        if (campaign != null)
        {
            campaign.IsDeleted = true;
            await _db.SaveChangesAsync();
        }
    }
}
