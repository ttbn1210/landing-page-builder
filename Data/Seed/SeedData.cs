using TrackQR.Web.Models;

namespace TrackQR.Web.Data.Seed;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Campaigns.Any()) return;

        var campaign1Id = Guid.NewGuid();
        var campaign2Id = Guid.NewGuid();

        var campaigns = new List<Campaign>
        {
            new()
            {
                Id = campaign1Id,
                Name = "Summer Sale 2024",
                Description = "Summer promotion across all locations",
                Status = CampaignStatus.Active,
                StartDate = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2024, 8, 31, 0, 0, 0, DateTimeKind.Utc),
                Budget = 5000m
            },
            new()
            {
                Id = campaign2Id,
                Name = "Back to School",
                Description = "School banner campaign targeting students",
                Status = CampaignStatus.Draft,
                StartDate = new DateTime(2024, 8, 15, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2024, 9, 30, 0, 0, 0, DateTimeKind.Utc),
                Budget = 3000m
            }
        };

        context.Campaigns.AddRange(campaigns);

        var placement1Id = Guid.NewGuid();
        var placement2Id = Guid.NewGuid();
        var placement3Id = Guid.NewGuid();

        var placements = new List<Placement>
        {
            new()
            {
                Id = placement1Id,
                CampaignId = campaign1Id,
                Name = "Car Sticker - Toyota Vios",
                LocationNote = "Back window of delivery vehicle, District 1 route",
                Cost = 200m
            },
            new()
            {
                Id = placement2Id,
                CampaignId = campaign1Id,
                Name = "Restaurant Table Tent - Phở 24",
                LocationNote = "Table tent at Phở 24, Nguyễn Huệ branch, 20 tables",
                Cost = 150m
            },
            new()
            {
                Id = placement3Id,
                CampaignId = campaign2Id,
                Name = "School Banner - HCMUS",
                LocationNote = "Main entrance banner at HCMUS campus",
                Cost = 500m
            }
        };

        context.Placements.AddRange(placements);

        var landingPage1Id = Guid.NewGuid();
        var landingPage2Id = Guid.NewGuid();

        var landingPages = new List<LandingPage>
        {
            new()
            {
                Id = landingPage1Id,
                Name = "Summer Sale Landing",
                TemplateType = TemplateType.LeadForm,
                Status = LandingPageStatus.Published,
                HtmlContent = GetSummerSaleHtml(),
                CssContent = GetDefaultCss()
            },
            new()
            {
                Id = landingPage2Id,
                Name = "School Promotion",
                TemplateType = TemplateType.SimpleHero,
                Status = LandingPageStatus.Published,
                HtmlContent = GetSchoolPromoHtml(),
                CssContent = GetDefaultCss()
            }
        };

        context.LandingPages.AddRange(landingPages);

        var qrCodes = new List<QRCode>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CampaignId = campaign1Id,
                PlacementId = placement1Id,
                LandingPageId = landingPage1Id,
                ShortCode = "summer-car-d1",
                Status = QRCodeStatus.Active,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                CampaignId = campaign1Id,
                PlacementId = placement2Id,
                LandingPageId = landingPage1Id,
                ShortCode = "summer-pho24",
                Status = QRCodeStatus.Active,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                CampaignId = campaign2Id,
                PlacementId = placement3Id,
                LandingPageId = landingPage2Id,
                ShortCode = "school-hcmus",
                Status = QRCodeStatus.Active,
                IsActive = true
            }
        };

        context.QRCodes.AddRange(qrCodes);
        context.SaveChanges();
    }

    private static string GetSummerSaleHtml()
    {
        return """
        <!DOCTYPE html>
        <html lang="vi">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Khuyến Mãi Mùa Hè 2024</title>
        </head>
        <body>
            <div class="hero">
                <h1>🔥 Khuyến Mãi Mùa Hè</h1>
                <p>Giảm đến 50% tất cả sản phẩm</p>
                <a href="#form" class="cta-btn" data-track="button-click" data-element="hero-cta">Nhận Ưu Đãi Ngay</a>
            </div>
            <div id="form" class="lead-form">
                <h2>Đăng Ký Nhận Ưu Đãi</h2>
                <form id="leadForm">
                    <input type="text" name="fullName" placeholder="Họ và tên" required />
                    <input type="tel" name="phone" placeholder="Số điện thoại" required />
                    <input type="email" name="email" placeholder="Email" />
                    <textarea name="message" placeholder="Ghi chú"></textarea>
                    <button type="submit" data-track="form-submit" data-element="lead-form">Gửi Đăng Ký</button>
                </form>
            </div>
            <div class="call-section">
                <a href="tel:0901234567" class="call-btn" data-track="call-click" data-element="call-hotline">📞 Gọi Hotline: 090 123 4567</a>
            </div>
        </body>
        </html>
        """;
    }

    private static string GetSchoolPromoHtml()
    {
        return """
        <!DOCTYPE html>
        <html lang="vi">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Back to School 2024</title>
        </head>
        <body>
            <div class="hero">
                <h1>📚 Back to School 2024</h1>
                <p>Ưu đãi dành riêng cho sinh viên</p>
                <a href="#form" class="cta-btn" data-track="button-click" data-element="hero-cta">Xem Chi Tiết</a>
            </div>
            <div id="form" class="lead-form">
                <h2>Đăng Ký Tư Vấn</h2>
                <form id="leadForm">
                    <input type="text" name="fullName" placeholder="Họ và tên" required />
                    <input type="tel" name="phone" placeholder="Số điện thoại" required />
                    <input type="email" name="email" placeholder="Email sinh viên" />
                    <textarea name="message" placeholder="Bạn quan tâm điều gì?"></textarea>
                    <button type="submit" data-track="form-submit" data-element="lead-form">Đăng Ký Ngay</button>
                </form>
            </div>
        </body>
        </html>
        """;
    }

    private static string GetDefaultCss()
    {
        return """
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { font-family: 'Segoe UI', sans-serif; color: #333; }
        .hero { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 60px 20px; text-align: center; }
        .hero h1 { font-size: 2.5rem; margin-bottom: 15px; }
        .hero p { font-size: 1.2rem; margin-bottom: 30px; }
        .cta-btn { display: inline-block; background: #ff6b35; color: white; padding: 15px 40px; border-radius: 30px; text-decoration: none; font-weight: bold; font-size: 1.1rem; }
        .lead-form { max-width: 500px; margin: 40px auto; padding: 30px; }
        .lead-form h2 { margin-bottom: 20px; text-align: center; }
        .lead-form input, .lead-form textarea { width: 100%; padding: 12px; margin-bottom: 15px; border: 1px solid #ddd; border-radius: 8px; font-size: 1rem; }
        .lead-form button { width: 100%; padding: 15px; background: #28a745; color: white; border: none; border-radius: 8px; font-size: 1.1rem; cursor: pointer; }
        .call-section { text-align: center; padding: 30px; }
        .call-btn { display: inline-block; background: #007bff; color: white; padding: 15px 30px; border-radius: 30px; text-decoration: none; font-size: 1.1rem; }
        """;
    }
}
