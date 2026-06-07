using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LandingPageBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller}")]
    [Authorize]
    public class ComponentsApiController : ControllerBase
    {
        private readonly IPageComponentRepository _componentRepository;
        private readonly ILandingPageRepository _pageRepository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ComponentsApiController> _logger;

        public ComponentsApiController(
            IPageComponentRepository componentRepository,
            ILandingPageRepository pageRepository,
            ApplicationDbContext context,
            ILogger<ComponentsApiController> logger)
        {
            _componentRepository = componentRepository;
            _pageRepository = pageRepository;
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<PageComponent>> CreateComponent([FromBody] PageComponentDto dto)
        {
            var page = await _pageRepository.GetByIdAsync(dto.LandingPageId);
            if (page == null)
                return NotFound("Page not found");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page.UserId != userId)
                return Forbid();

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

            var page = await _pageRepository.GetByIdAsync(component.LandingPageId);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page?.UserId != userId)
                return Forbid();

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

            var page = await _pageRepository.GetByIdAsync(component.LandingPageId);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page?.UserId != userId)
                return Forbid();

            await _componentRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id:int}/duplicate")]
        public async Task<ActionResult<PageComponent>> DuplicateComponent(int id)
        {
            var component = await _componentRepository.GetByIdAsync(id);
            if (component == null)
                return NotFound();

            var page = await _pageRepository.GetByIdAsync(component.LandingPageId);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page?.UserId != userId)
                return Forbid();

            var components = await _componentRepository.GetByPageIdAsync(component.LandingPageId);
            var maxOrder = components.Max(c => c.Order);

            var newComponent = new PageComponent
            {
                LandingPageId = component.LandingPageId,
                ComponentName = component.ComponentName,
                Content = component.Content,
                Order = maxOrder + 1,
                CssClass = component.CssClass
            };

            await _componentRepository.CreateAsync(newComponent);
            return CreatedAtAction(nameof(GetComponent), new { id = newComponent.Id }, newComponent);
        }

        [HttpPost("{id:int}/reorder")]
        public async Task<IActionResult> ReorderComponent(int id, [FromBody] ReorderDto dto)
        {
            var component = await _componentRepository.GetByIdAsync(id);
            if (component == null)
                return NotFound();

            var page = await _pageRepository.GetByIdAsync(component.LandingPageId);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page?.UserId != userId)
                return Forbid();

            var allComponents = (await _componentRepository.GetByPageIdAsync(component.LandingPageId))
                .OrderBy(c => c.Order)
                .ToList();

            var currentIndex = allComponents.FindIndex(c => c.Id == id);

            if (dto.Direction == "up" && currentIndex > 0)
            {
                var temp = allComponents[currentIndex].Order;
                allComponents[currentIndex].Order = allComponents[currentIndex - 1].Order;
                allComponents[currentIndex - 1].Order = temp;

                _context.PageComponents.Update(allComponents[currentIndex]);
                _context.PageComponents.Update(allComponents[currentIndex - 1]);
            }
            else if (dto.Direction == "down" && currentIndex < allComponents.Count - 1)
            {
                var temp = allComponents[currentIndex].Order;
                allComponents[currentIndex].Order = allComponents[currentIndex + 1].Order;
                allComponents[currentIndex + 1].Order = temp;

                _context.PageComponents.Update(allComponents[currentIndex]);
                _context.PageComponents.Update(allComponents[currentIndex + 1]);
            }

            await _context.SaveChangesAsync();
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

    public class ReorderDto
    {
        public string Direction { get; set; } = string.Empty; // "up" or "down"
    }
}
