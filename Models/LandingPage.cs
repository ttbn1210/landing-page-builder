namespace TrackQR.Web.Models;

public class LandingPage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public TemplateType TemplateType { get; set; } = TemplateType.SimpleHero;
    public string? HtmlContent { get; set; }
    public string? CssContent { get; set; }
    public string? HeadScripts { get; set; }
    public string? BodyScripts { get; set; }
    public LandingPageStatus Status { get; set; } = LandingPageStatus.Draft;

    public ICollection<QRCode> QRCodes { get; set; } = new List<QRCode>();
}
