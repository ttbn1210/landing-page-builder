using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LandingPageBuilder.Pages.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ILandingPageRepository _pageRepository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;

        public LandingPage LandingPage { get; set; } = new();
        public List<ComponentType> ComponentTypes { get; set; } = new();

        public EditModel(ILandingPageRepository pageRepository, ApplicationDbContext context, ILogger<EditModel> logger)
        {
            _pageRepository = pageRepository;
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
            await _pageRepository.UpdateAsync(page);

            return RedirectToPage(new { id = page.Id });
        }
    }
}
