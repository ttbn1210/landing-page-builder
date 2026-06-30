using Microsoft.AspNetCore.Http;

namespace TrackQR.Web.Services.Media;

public class MediaFileInfo
{
    public string FileName { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Url { get; set; } = string.Empty;
}

public class MediaFileService
{
    private readonly IWebHostEnvironment _env;
    private const string MediaFolder = "uploads";

    public MediaFileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    private string GetMediaPath()
    {
        var path = Path.Combine(_env.WebRootPath, MediaFolder);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }

    public List<MediaFileInfo> GetAllFiles()
    {
        var mediaPath = GetMediaPath();
        var files = new List<MediaFileInfo>();

        foreach (var filePath in Directory.GetFiles(mediaPath, "*.*", SearchOption.AllDirectories))
        {
            var fileInfo = new FileInfo(filePath);
            var relativePath = Path.GetRelativePath(_env.WebRootPath, filePath).Replace("\\", "/");

            files.Add(new MediaFileInfo
            {
                FileName = fileInfo.Name,
                RelativePath = relativePath,
                ContentType = GetContentType(fileInfo.Extension),
                Size = fileInfo.Length,
                LastModified = fileInfo.LastWriteTimeUtc,
                Url = "/" + relativePath
            });
        }

        return files.OrderByDescending(f => f.LastModified).ToList();
    }

    public async Task<MediaFileInfo?> UploadAsync(IFormFile file)
    {
        if (file == null || file.Length == 0) return null;

        var mediaPath = GetMediaPath();

        // Sanitize filename
        var fileName = SanitizeFileName(file.FileName);

        // Avoid overwriting - add timestamp if file exists
        var targetPath = Path.Combine(mediaPath, fileName);
        if (File.Exists(targetPath))
        {
            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);
            fileName = $"{nameWithoutExt}_{DateTime.UtcNow:yyyyMMddHHmmss}{ext}";
            targetPath = Path.Combine(mediaPath, fileName);
        }

        await using (var stream = new FileStream(targetPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileInfo = new FileInfo(targetPath);
        var relativePath = Path.GetRelativePath(_env.WebRootPath, targetPath).Replace("\\", "/");

        return new MediaFileInfo
        {
            FileName = fileInfo.Name,
            RelativePath = relativePath,
            ContentType = GetContentType(fileInfo.Extension),
            Size = fileInfo.Length,
            LastModified = fileInfo.LastWriteTimeUtc,
            Url = "/" + relativePath
        };
    }

    public bool Delete(string fileName)
    {
        var mediaPath = GetMediaPath();
        var filePath = Path.Combine(mediaPath, fileName);

        // Prevent path traversal
        if (!Path.GetFullPath(filePath).StartsWith(Path.GetFullPath(mediaPath)))
            return false;

        if (!File.Exists(filePath)) return false;

        File.Delete(filePath);
        return true;
    }

    public string GetPublicUrl(string fileName)
    {
        return $"/{MediaFolder}/{fileName}";
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalid, StringSplitOptions.RemoveEmptyEntries));
        return sanitized.Replace(" ", "_").ToLowerInvariant();
    }

    private static string GetContentType(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            ".pdf" => "application/pdf",
            ".mp4" => "video/mp4",
            ".webm" => "video/webm",
            ".doc" or ".docx" => "application/msword",
            ".xls" or ".xlsx" => "application/vnd.ms-excel",
            ".zip" => "application/zip",
            _ => "application/octet-stream"
        };
    }
}
