namespace LandingPageBuilder.Models
{
    public class MediaFile
    {
        public int Id { get; set; }
        public int LandingPageId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty; // image, video, document
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty;

        // Navigation properties
        public LandingPage? LandingPage { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
