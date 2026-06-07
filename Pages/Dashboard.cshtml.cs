using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LandingPageBuilder.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly ILandingPageRepository _pageRepository;
        private readonly ILogger<DashboardModel> _logger;

        public IEnumerable<LandingPage> LandingPages { get; set; } = new List<LandingPage>();

        public DashboardModel(ILandingPageRepository pageRepository, ILogger<DashboardModel> logger)
        {
            _pageRepository = pageRepository;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                LandingPages = await _pageRepository.GetUserPagesAsync(userId);
            }
        }
    }
}
