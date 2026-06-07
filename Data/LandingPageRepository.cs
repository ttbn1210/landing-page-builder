using LandingPageBuilder.Models;
using Microsoft.EntityFrameworkCore;

namespace LandingPageBuilder.Data
{
    public class LandingPageRepository : ILandingPageRepository
    {
        private readonly ApplicationDbContext _context;

        public LandingPageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LandingPage?> GetByIdAsync(int id)
        {
            return await _context.LandingPages
                .Include(p => p.Components)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<LandingPage?> GetBySlugAsync(string slug)
        {
            return await _context.LandingPages
                .Include(p => p.Components.OrderBy(c => c.Order))
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
        }

        public async Task<IEnumerable<LandingPage>> GetUserPagesAsync(string userId)
        {
            return await _context.LandingPages
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<LandingPage>> GetPublishedPagesAsync()
        {
            return await _context.LandingPages
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<LandingPage> CreateAsync(LandingPage page)
        {
            _context.LandingPages.Add(page);
            await _context.SaveChangesAsync();
            return page;
        }

        public async Task<LandingPage> UpdateAsync(LandingPage page)
        {
            page.UpdatedAt = DateTime.UtcNow;
            _context.LandingPages.Update(page);
            await _context.SaveChangesAsync();
            return page;
        }

        public async Task DeleteAsync(int id)
        {
            var page = await _context.LandingPages.FindAsync(id);
            if (page != null)
            {
                _context.LandingPages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
        {
            var query = _context.LandingPages.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }
            return await query.AnyAsync(p => p.Slug == slug);
        }
    }
}
