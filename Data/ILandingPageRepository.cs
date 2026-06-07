using LandingPageBuilder.Models;

namespace LandingPageBuilder.Data
{
    public interface ILandingPageRepository
    {
        Task<LandingPage?> GetByIdAsync(int id);
        Task<LandingPage?> GetBySlugAsync(string slug);
        Task<IEnumerable<LandingPage>> GetUserPagesAsync(string userId);
        Task<IEnumerable<LandingPage>> GetPublishedPagesAsync();
        Task<LandingPage> CreateAsync(LandingPage page);
        Task<LandingPage> UpdateAsync(LandingPage page);
        Task DeleteAsync(int id);
        Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    }
}
