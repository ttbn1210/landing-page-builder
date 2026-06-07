using LandingPageBuilder.Data;
using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LandingPageBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UploadApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UploadApiController> _logger;
        private readonly string _uploadPath;

        public UploadApiController(ApplicationDbContext context, ILogger<UploadApiController> logger, IWebHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            _uploadPath = Path.Combine(env.WebRootPath, "uploads");
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile([FromForm] IFormFile file, [FromForm] int? pageId = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            try
            {
                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf", ".doc", ".docx" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    return BadRequest("File type not allowed");

                // Validate file size (max 10MB)
                if (file.Length > 10 * 1024 * 1024)
                    return BadRequest("File is too large (max 10MB)");

                // Create uploads directory if it doesn't exist
                if (!Directory.Exists(_uploadPath))
                    Directory.CreateDirectory(_uploadPath);

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_uploadPath, fileName);

                // Save file
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"/uploads/{fileName}";
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Save to database if page ID provided
                if (pageId.HasValue && !string.IsNullOrEmpty(userId))
                {
                    var mediaFile = new MediaFile
                    {
                        LandingPageId = pageId.Value,
                        FileName = file.FileName,
                        FileUrl = fileUrl,
                        FileType = extension.Contains("image") ? "image" : "document",
                        FileSize = file.Length,
                        UserId = userId
                    };

                    _context.MediaFiles.Add(mediaFile);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { url = fileUrl, fileName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                return StatusCode(500, "Error uploading file");
            }
        }
    }
}
