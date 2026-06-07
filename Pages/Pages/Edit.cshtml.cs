using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

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
            page.IsPublished = bool.Parse(Request.Form["IsPublished"]);
            page.MetaDescription = LandingPage.MetaDescription;
            page.LogoUrl = LandingPage.LogoUrl;
            page.HeaderColor = LandingPage.HeaderColor;

            await _pageRepository.UpdateAsync(page);

            return RedirectToPage(new { id = page.Id });
        }

        public async Task<IActionResult> OnGetRenderComponentAsync(int componentId)
        {
            var component = await _componentRepository.GetByIdAsync(componentId);
            if (component == null)
                return NotFound();

            var page = await _pageRepository.GetByIdAsync(component.LandingPageId);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page?.UserId != userId)
                return Forbid();

            // Return the rendered HTML for preview
            var html = RenderComponentHtml(component);
            return Content(html, "text/html");
        }

        private string RenderComponentHtml(PageComponent component)
        {
            try
            {
                var data = JsonSerializer.Deserialize<JsonElement>(component.Content);

                return component.ComponentName switch
                {
                    "Hero" => RenderHero(data),
                    "Features" => RenderFeatures(data),
                    "Testimonials" => RenderTestimonials(data),
                    "CallToAction" => RenderCTA(data),
                    "TextContent" => RenderTextContent(data),
                    "Gallery" => RenderGallery(data),
                    "Newsletter" => RenderNewsletter(data),
                    "ContactForm" => RenderContactForm(data),
                    _ => "<p>Unknown component type</p>"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rendering component");
                return $"<p class='alert alert-danger'>Error rendering component: {ex.Message}</p>";
            }
        }

        private string RenderHero(JsonElement data)
        {
            var headline = data.TryGetProperty("headline", out var h) ? h.GetString() : "Welcome";
            var subheadline = data.TryGetProperty("subheadline", out var s) ? s.GetString() : "";
            var bgImage = data.TryGetProperty("backgroundImage", out var b) ? b.GetString() : "";
            var bgStyle = !string.IsNullOrEmpty(bgImage) ? $"background-image: url('{bgImage}'); background-size: cover; background-position: center;" : "background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);";

            return $@"
                <section style="{bgStyle} padding: 80px 20px; color: white; text-align: center;">
                    <h1 style="font-size: 3rem; margin: 0 0 20px 0;">{headline}</h1>
                    <p style="font-size: 1.2rem; margin: 0;">{subheadline}</p>
                </section>
            ";
        }

        private string RenderFeatures(JsonElement data)
        {
            var title = data.TryGetProperty("title", out var t) ? t.GetString() : "Features";
            var items = data.TryGetProperty("items", out var i) ? i.EnumerateArray().ToList() : new List<JsonElement>();

            var itemsHtml = string.Join("", items.Select(item =>
            {
                var icon = item.TryGetProperty("icon", out var ic) ? ic.GetString() : "⭐";
                var itemTitle = item.TryGetProperty("title", out var it) ? it.GetString() : "Feature";
                var description = item.TryGetProperty("description", out var d) ? d.GetString() : "";

                return $@"
                    <div style="background: white; padding: 30px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); text-align: center;">
                        <div style="font-size: 2.5rem; margin-bottom: 15px;">{icon}</div>
                        <h3 style="margin: 15px 0 10px 0;">{itemTitle}</h3>
                        <p style="margin: 0; color: #666;">{description}</p>
                    </div>
                ";
            }));

            return $@"
                <section style="padding: 60px 20px; background: #f8f9fa;">
                    <h2 style="text-align: center; font-size: 2rem; margin: 0 0 40px 0;">{title}</h2>
                    <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 30px; max-width: 1200px; margin: 0 auto;">
                        {itemsHtml}
                    </div>
                </section>
            ";
        }

        private string RenderTestimonials(JsonElement data)
        {
            var title = data.TryGetProperty("title", out var t) ? t.GetString() : "Testimonials";
            var testimonials = data.TryGetProperty("testimonials", out var tm) ? tm.EnumerateArray().ToList() : new List<JsonElement>();

            var testimonialsHtml = string.Join("", testimonials.Select(testimonial =>
            {
                var name = testimonial.TryGetProperty("name", out var n) ? n.GetString() : "Anonymous";
                var company = testimonial.TryGetProperty("company", out var c) ? c.GetString() : "";
                var text = testimonial.TryGetProperty("text", out var txt) ? txt.GetString() : "";

                return $@"
                    <div style="background: #f8f9fa; padding: 30px; border-radius: 8px; border-left: 4px solid #007bff;">
                        <p style="font-style: italic; margin: 0 0 15px 0;\">\"<strong>{text}</strong>\"</p>
                        <div style="font-weight: bold; color: #333;">{name}</div>
                        <small style="color: #666;">{company}</small>
                    </div>
                ";
            }));

            return $@"
                <section style="padding: 60px 20px; background: white;">
                    <h2 style="text-align: center; font-size: 2rem; margin: 0 0 40px 0;">{title}</h2>
                    <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 30px; max-width: 1200px; margin: 0 auto;">
                        {testimonialsHtml}
                    </div>
                </section>
            ";
        }

        private string RenderCTA(JsonElement data)
        {
            var headline = data.TryGetProperty("headline", out var h) ? h.GetString() : "Call to Action";
            var description = data.TryGetProperty("description", out var d) ? d.GetString() : "";
            var bgColor = data.TryGetProperty("backgroundColor", out var bg) ? bg.GetString() : "#007bff";

            return $@"
                <section style="background: linear-gradient(135deg, {bgColor} 0%, {bgColor} 100%); padding: 80px 20px; color: white; text-align: center;">
                    <h2 style="font-size: 2.5rem; margin: 0 0 20px 0;">{headline}</h2>
                    <p style="font-size: 1.1rem; margin: 0 0 30px 0;">{description}</p>
                </section>
            ";
        }

        private string RenderTextContent(JsonElement data)
        {
            var content = data.TryGetProperty("content", out var c) ? c.GetString() : "";
            return $@"
                <section style="padding: 60px 20px; background: white;">
                    <div style="max-width: 1200px; margin: 0 auto;">
                        {content}
                    </div>
                </section>
            ";
        }

        private string RenderGallery(JsonElement data)
        {
            var title = data.TryGetProperty("title", out var t) ? t.GetString() : "Gallery";
            var images = data.TryGetProperty("images", out var i) ? i.EnumerateArray().ToList() : new List<JsonElement>();

            var imagesHtml = string.Join("", images.Select(image =>
            {
                var imageUrl = image.ValueKind == JsonValueKind.String ? image.GetString() : "";
                return !string.IsNullOrEmpty(imageUrl)
                    ? $@"<div><img src=\"{imageUrl}\" style=\"width: 100%; height: 250px; object-fit: cover; border-radius: 8px;\" alt=\"Gallery item\"></div>"
                    : "";
            }));

            return $@"
                <section style="padding: 60px 20px; background: #f8f9fa;">
                    <h2 style="text-align: center; font-size: 2rem; margin: 0 0 40px 0;">{title}</h2>
                    <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 20px; max-width: 1200px; margin: 0 auto;">
                        {imagesHtml}
                    </div>
                </section>
            ";
        }

        private string RenderNewsletter(JsonElement data)
        {
            var title = data.TryGetProperty("title", out var t) ? t.GetString() : "Newsletter";
            var description = data.TryGetProperty("description", out var d) ? d.GetString() : "";
            var placeholder = data.TryGetProperty("placeholder", out var p) ? p.GetString() : "Enter your email";
            var buttonText = data.TryGetProperty("buttonText", out var b) ? b.GetString() : "Subscribe";

            return $@"
                <section style="padding: 60px 20px; background: white;">
                    <div style="max-width: 800px; margin: 0 auto; text-align: center;">
                        <h2 style="font-size: 2rem; margin: 0 0 20px 0;">{title}</h2>
                        <p style="margin: 0 0 30px 0;">{description}</p>
                        <form style="display: flex; gap: 10px; justify-content: center;">
                            <input type=\"email\" placeholder=\"{placeholder}\" style=\"padding: 12px; border: 1px solid #ddd; border-radius: 5px; width: 300px;\" required>
                            <button type=\"submit\" style=\"padding: 12px 30px; background: #007bff; color: white; border: none; border-radius: 5px; cursor: pointer; font-weight: bold;\">{buttonText}</button>
                        </form>
                    </div>
                </section>
            ";
        }

        private string RenderContactForm(JsonElement data)
        {
            var title = data.TryGetProperty("title", out var t) ? t.GetString() : "Contact Us";
            var description = data.TryGetProperty("description", out var d) ? d.GetString() : "";

            return $@"
                <section style="padding: 60px 20px; background: white;">
                    <div style="max-width: 600px; margin: 0 auto;">
                        <h2 style="text-align: center; font-size: 2rem; margin: 0 0 20px 0;">{title}</h2>
                        <p style="text-align: center; margin: 0 0 30px 0;">{description}</p>
                        <form style="display: flex; flex-direction: column; gap: 15px;">
                            <input type=\"text\" placeholder=\"Your Name\" style=\"padding: 10px; border: 1px solid #ddd; border-radius: 5px;\" required>
                            <input type=\"email\" placeholder=\"Your Email\" style=\"padding: 10px; border: 1px solid #ddd; border-radius: 5px;\" required>
                            <textarea placeholder=\"Message\" rows=\"5\" style=\"padding: 10px; border: 1px solid #ddd; border-radius: 5px; font-family: inherit;\" required></textarea>
                            <button type=\"submit\" style=\"padding: 12px; background: #007bff; color: white; border: none; border-radius: 5px; cursor: pointer; font-weight: bold;\" required>Send Message</button>
                        </form>
                    </div>
                </section>
            ";
        }
    }
}
