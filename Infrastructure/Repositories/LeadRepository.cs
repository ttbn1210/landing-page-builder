using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;

namespace TrackQR.Web.Infrastructure.Repositories;

public class LeadRepository
{
    private readonly AppDbContext _db;

    public LeadRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Lead>> GetAllAsync()
    {
        return await _db.Leads
            .Include(l => l.Session)
                .ThenInclude(s => s.QRCode)
                    .ThenInclude(q => q.Campaign)
            .Include(l => l.Activities)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<Lead?> GetByIdAsync(Guid id)
    {
        return await _db.Leads
            .Include(l => l.Session)
                .ThenInclude(s => s.QRCode)
                    .ThenInclude(q => q.Campaign)
            .Include(l => l.Activities)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Lead> CreateAsync(Lead lead)
    {
        _db.Leads.Add(lead);
        await _db.SaveChangesAsync();
        return lead;
    }

    public async Task UpdateAsync(Lead lead)
    {
        _db.Leads.Update(lead);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Lead>> GetByStatusAsync(LeadStatus status)
    {
        return await _db.Leads
            .Where(l => l.Status == status)
            .Include(l => l.Session)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }
}
