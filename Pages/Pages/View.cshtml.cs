using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LandingPageBuilder.Pages.Pages
{
    public class ViewModel : PageModel
    {
        private readonly ILandingPageRepository _pageRepository;

        public LandingPage? LandingPage { get; set; }

        public ViewModel(ILandingPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task OnGetAsync(string slug)
        {
            LandingPage = await _pageRepository.GetBySlugAsync(slug);
        }
    }
}
