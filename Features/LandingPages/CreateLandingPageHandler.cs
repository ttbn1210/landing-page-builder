using TrackQR.Web.Data;
using TrackQR.Web.Models;
using TrackQR.Web.Services.Landing;

namespace TrackQR.Web.Features.LandingPages;

public class CreateLandingPageHandler
{
    private readonly AppDbContext _db;
    private readonly TemplateService _templateService;

    public CreateLandingPageHandler(AppDbContext db, TemplateService templateService)
    {
        _db = db;
        _templateService = templateService;
    }

    public async Task<LandingPage> HandleAsync(string name, TemplateType templateType, string? htmlContent = null)
    {
        var page = new LandingPage
        {
            Name = name,
            TemplateType = templateType,
            HtmlContent = htmlContent ?? _templateService.GetTemplate(templateType),
            Status = LandingPageStatus.Draft
        };

        _db.LandingPages.Add(page);
        await _db.SaveChangesAsync();

        return page;
    }
}
