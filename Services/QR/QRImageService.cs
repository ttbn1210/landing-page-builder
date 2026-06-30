using QRCoder;

namespace TrackQR.Web.Services.QR;

public class QRImageService
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public QRImageService(IConfiguration config, IWebHostEnvironment env)
    {
        _config = config;
        _env = env;
    }

    public string GenerateImage(string shortCode)
    {
        var baseUrl = _config["App:BaseUrl"] ?? "https://localhost:5001";
        var url = $"{baseUrl}/{shortCode}";

        using var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrData);
        var qrBytes = qrCode.GetGraphic(20);

        var directory = Path.Combine(_env.WebRootPath, "qrcodes");
        Directory.CreateDirectory(directory);

        var filePath = Path.Combine(directory, $"{shortCode}.png");
        File.WriteAllBytes(filePath, qrBytes);

        return $"/qrcodes/{shortCode}.png";
    }

    public string GetImagePath(string shortCode)
    {
        return $"/qrcodes/{shortCode}.png";
    }

    public bool ImageExists(string shortCode)
    {
        var filePath = Path.Combine(_env.WebRootPath, "qrcodes", $"{shortCode}.png");
        return File.Exists(filePath);
    }
}
