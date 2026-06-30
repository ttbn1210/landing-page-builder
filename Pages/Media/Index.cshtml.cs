using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrackQR.Web.Services.Media;

namespace TrackQR.Web.Pages.Media;

public class IndexModel : PageModel
{
    private readonly MediaFileService _mediaService;

    public IndexModel(MediaFileService mediaService)
    {
        _mediaService = mediaService;
    }

    public List<MediaFileInfo> Files { get; set; } = new();

    public void OnGet()
    {
        Files = _mediaService.GetAllFiles();
    }

    public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "Vui lòng chọn file để upload.");
            Files = _mediaService.GetAllFiles();
            return Page();
        }

        // Limit file size to 10MB
        if (file.Length > 10 * 1024 * 1024)
        {
            ModelState.AddModelError("", "File không được vượt quá 10MB.");
            Files = _mediaService.GetAllFiles();
            return Page();
        }

        await _mediaService.UploadAsync(file);
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(string fileName)
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            _mediaService.Delete(fileName);
        }
        return RedirectToPage();
    }
}
