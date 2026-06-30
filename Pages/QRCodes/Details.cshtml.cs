using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Data;
using TrackQR.Web.Models;
using TrackQR.Web.Services.QR;

namespace TrackQR.Web.Pages.QRCodes;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly QRImageService _imageService;
    private readonly IConfiguration _config;

    public DetailsModel(AppDbContext db, QRImageService imageService, IConfiguration config)
    {
        _db = db;
        _imageService = imageService;
        _config = config;
    }

    public QRCode? QRCode { get; set; }
    public bool ImageExists { get; set; }
    public string BaseUrl => _config["App:BaseUrl"] ?? "https://localhost:5001";

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        QRCode = await _db.QRCodes
            .Include(q => q.Campaign)
            .Include(q => q.Placement)
            .Include(q => q.LandingPage)
            .Include(q => q.VisitorSessions)
            .Include(q => q.ScanEvents)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (QRCode == null) return NotFound();

        ImageExists = _imageService.ImageExists(QRCode.ShortCode);
        return Page();
    }
}
