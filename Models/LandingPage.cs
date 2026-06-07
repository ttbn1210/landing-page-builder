namespace LandingPageBuilder.Models
{
    public class LandingPage
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public string? FaviconUrl { get; set; }
        public string? LogoUrl { get; set; }
        public string? HeaderColor { get; set; } = "#ffffff";
        public string? FooterColor { get; set; } = "#f8f9fa";
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty;

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public ICollection<PageComponent> Components { get; set; } = new List<PageComponent>();
    }
}
