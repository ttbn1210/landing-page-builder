using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LandingPageBuilder.Pages.Pages
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ILandingPageRepository _pageRepository;
        private readonly ILogger<CreateModel> _logger;

        [BindProperty]
        public LandingPage LandingPage { get; set; } = new();

        public CreateModel(ILandingPageRepository pageRepository, ILogger<CreateModel> logger)
        {
            _pageRepository = pageRepository;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validate slug
            if (string.IsNullOrWhiteSpace(LandingPage.Slug))
            {
                LandingPage.Slug = LandingPage.Title.ToLower().Replace(" ", "-");
            }

            if (await _pageRepository.SlugExistsAsync(LandingPage.Slug))
            {
                ModelState.AddModelError("LandingPage.Slug", "This slug is already taken.");
                return Page();
            }

            LandingPage.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            LandingPage.CreatedAt = DateTime.UtcNow;
            LandingPage.UpdatedAt = DateTime.UtcNow;

            await _pageRepository.CreateAsync(LandingPage);

            return RedirectToPage("./Edit", new { id = LandingPage.Id });
        }
    }
}
