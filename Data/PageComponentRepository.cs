using LandingPageBuilder.Models;
using Microsoft.EntityFrameworkCore;

namespace LandingPageBuilder.Data
{
    public class PageComponentRepository : IPageComponentRepository
    {
        private readonly ApplicationDbContext _context;

        public PageComponentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PageComponent?> GetByIdAsync(int id)
        {
            return await _context.PageComponents.FindAsync(id);
        }

        public async Task<IEnumerable<PageComponent>> GetByPageIdAsync(int pageId)
        {
            return await _context.PageComponents
                .Where(c => c.LandingPageId == pageId)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }

        public async Task<PageComponent> CreateAsync(PageComponent component)
        {
            _context.PageComponents.Add(component);
            await _context.SaveChangesAsync();
            return component;
        }

        public async Task<PageComponent> UpdateAsync(PageComponent component)
        {
            component.UpdatedAt = DateTime.UtcNow;
            _context.PageComponents.Update(component);
            await _context.SaveChangesAsync();
            return component;
        }

        public async Task DeleteAsync(int id)
        {
            var component = await _context.PageComponents.FindAsync(id);
            if (component != null)
            {
                _context.PageComponents.Remove(component);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReorderComponentsAsync(int pageId, List<(int Id, int Order)> orders)
        {
            foreach (var (id, order) in orders)
            {
                var component = await _context.PageComponents.FindAsync(id);
                if (component != null && component.LandingPageId == pageId)
                {
                    component.Order = order;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
