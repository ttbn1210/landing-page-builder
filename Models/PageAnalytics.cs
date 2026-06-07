namespace LandingPageBuilder.Models
{
    public class PageAnalytics
    {
        public int Id { get; set; }
        public int LandingPageId { get; set; }
        public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
        public string? Referrer { get; set; }
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }
        public string? Country { get; set; }
        public string? Device { get; set; }

        // Navigation properties
        public LandingPage? LandingPage { get; set; }
    }
}
