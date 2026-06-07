namespace LandingPageBuilder.Models
{
    public class FormSubmission
    {
        public int Id { get; set; }
        public int LandingPageId { get; set; }
        public string FormName { get; set; } = string.Empty;
        public string SubmittedData { get; set; } = string.Empty; // JSON serialized
        public string? SubmitterEmail { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; }

        // Navigation properties
        public LandingPage? LandingPage { get; set; }
    }
}
