using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LandingPageBuilder.Pages.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ILandingPageRepository _pageRepository;
        private readonly IPageComponentRepository _componentRepository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;

        public LandingPage LandingPage { get; set; } = new();
        public List<ComponentType> ComponentTypes { get; set; } = new();
        public int PageViews { get; set; }

        public EditModel(
            ILandingPageRepository pageRepository,
            IPageComponentRepository componentRepository,
            ApplicationDbContext context,
            ILogger<EditModel> logger)
        {
            _pageRepository = pageRepository;
            _componentRepository = componentRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var page = await _pageRepository.GetByIdAsync(id);
            if (page == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page.UserId != userId)
                return Forbid();

            LandingPage = page;
            ComponentTypes = await _context.ComponentTypes.ToListAsync();
            PageViews = await _context.PageAnalytics.CountAsync(a => a.LandingPageId == id);

            return Page();
        }

        public async Task<IActionResult> OnPostSaveSettingsAsync()
        {
            var page = await _pageRepository.GetByIdAsync(LandingPage.Id);
            if (page == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page.UserId != userId)
                return Forbid();

            page.Title = LandingPage.Title;
            page.IsPublished = LandingPage.IsPublished;
            page.MetaDescription = LandingPage.MetaDescription;
            page.LogoUrl = LandingPage.LogoUrl;
            page.HeaderColor = LandingPage.HeaderColor;

            await _pageRepository.UpdateAsync(page);

            return RedirectToPage(new { id = page.Id });
        }
    }
}
