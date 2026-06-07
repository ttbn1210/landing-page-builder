using LandingPageBuilder.Models;

namespace LandingPageBuilder.Data
{
    public interface IPageComponentRepository
    {
        Task<PageComponent?> GetByIdAsync(int id);
        Task<IEnumerable<PageComponent>> GetByPageIdAsync(int pageId);
        Task<PageComponent> CreateAsync(PageComponent component);
        Task<PageComponent> UpdateAsync(PageComponent component);
        Task DeleteAsync(int id);
        Task ReorderComponentsAsync(int pageId, List<(int Id, int Order)> orders);
    }
}
