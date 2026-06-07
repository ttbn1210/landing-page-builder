using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPageBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ComponentsApiController : ControllerBase
    {
        private readonly IPageComponentRepository _componentRepository;
        private readonly ILandingPageRepository _pageRepository;
        private readonly ILogger<ComponentsApiController> _logger;

        public ComponentsApiController(
            IPageComponentRepository componentRepository,
            ILandingPageRepository pageRepository,
            ILogger<ComponentsApiController> logger)
        {
            _componentRepository = componentRepository;
            _pageRepository = pageRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<PageComponent>> CreateComponent([FromBody] PageComponentDto dto)
        {
            var page = await _pageRepository.GetByIdAsync(dto.LandingPageId);
            if (page == null)
                return NotFound("Page not found");

            var components = await _componentRepository.GetByPageIdAsync(dto.LandingPageId);
            var maxOrder = components.Any() ? components.Max(c => c.Order) : 0;

            var component = new PageComponent
            {
                LandingPageId = dto.LandingPageId,
                ComponentName = dto.ComponentName,
                Content = dto.Content,
                Order = maxOrder + 1
            };

            await _componentRepository.CreateAsync(component);
            return CreatedAtAction(nameof(GetComponent), new { id = component.Id }, component);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PageComponent>> GetComponent(int id)
        {
            var component = await _componentRepository.GetByIdAsync(id);
            if (component == null)
                return NotFound();

            return Ok(component);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComponent(int id, [FromBody] UpdateComponentDto dto)
        {
            var component = await _componentRepository.GetByIdAsync(id);
            if (component == null)
                return NotFound();

            component.Content = dto.Content;
            component.UpdatedAt = DateTime.UtcNow;

            await _componentRepository.UpdateAsync(component);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComponent(int id)
        {
            var component = await _componentRepository.GetByIdAsync(id);
            if (component == null)
                return NotFound();

            await _componentRepository.DeleteAsync(id);
            return NoContent();
        }
    }

    public class PageComponentDto
    {
        public int LandingPageId { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Order { get; set; }
    }

    public class UpdateComponentDto
    {
        public string Content { get; set; } = string.Empty;
    }
}
