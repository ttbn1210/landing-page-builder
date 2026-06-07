namespace LandingPageBuilder.Models
{
    public class PageComponent
    {
        public int Id { get; set; }
        public int LandingPageId { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; // JSON serialized content
        public int Order { get; set; }
        public bool IsVisible { get; set; } = true;
        public string? CssClass { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public LandingPage? LandingPage { get; set; }
    }
}
