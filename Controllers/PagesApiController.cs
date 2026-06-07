using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LandingPageBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PagesApiController : ControllerBase
    {
        private readonly ILandingPageRepository _pageRepository;
        private readonly ILogger<PagesApiController> _logger;

        public PagesApiController(ILandingPageRepository pageRepository, ILogger<PagesApiController> logger)
        {
            _pageRepository = pageRepository;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LandingPage>> GetPage(int id)
        {
            var page = await _pageRepository.GetByIdAsync(id);
            if (page == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page.UserId != userId)
                return Forbid();

            return Ok(page);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePage(int id)
        {
            var page = await _pageRepository.GetByIdAsync(id);
            if (page == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page.UserId != userId)
                return Forbid();

            await _pageRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
