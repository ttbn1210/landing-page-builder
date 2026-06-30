using TrackQR.Web.Models;

namespace TrackQR.Web.Services.Landing;

public class TemplateService
{
    public string GetTemplate(TemplateType templateType)
    {
        return templateType switch
        {
            TemplateType.SimpleHero => GetSimpleHeroTemplate(),
            TemplateType.LeadForm => GetLeadFormTemplate(),
            TemplateType.BookingPage => GetBookingTemplate(),
            TemplateType.ProductPage => GetProductTemplate(),
            _ => GetSimpleHeroTemplate()
        };
    }

    private static string GetSimpleHeroTemplate()
    {
        return """
        <!DOCTYPE html>
        <html lang="vi">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>{{title}}</title>
        </head>
        <body>
            <div class="hero">
                <h1>{{headline}}</h1>
                <p>{{subheadline}}</p>
                <a href="#contact" class="cta-btn" data-track="button-click" data-element="hero-cta">{{cta_text}}</a>
            </div>
            <div id="contact" class="contact-section">
                <a href="tel:{{phone}}" class="call-btn" data-track="call-click" data-element="call-main">📞 {{phone}}</a>
            </div>
        </body>
        </html>
        """;
    }

    private static string GetLeadFormTemplate()
    {
        return """
        <!DOCTYPE html>
        <html lang="vi">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>{{title}}</title>
        </head>
        <body>
            <div class="hero">
                <h1>{{headline}}</h1>
                <p>{{subheadline}}</p>
            </div>
            <div class="lead-form">
                <h2>{{form_title}}</h2>
                <form id="leadForm">
                    <input type="text" name="fullName" placeholder="Họ và tên" required />
                    <input type="tel" name="phone" placeholder="Số điện thoại" required />
                    <input type="email" name="email" placeholder="Email" />
                    <textarea name="message" placeholder="Ghi chú"></textarea>
                    <button type="submit" data-track="form-submit" data-element="lead-form">Gửi</button>
                </form>
            </div>
        </body>
        </html>
        """;
    }

    private static string GetBookingTemplate()
    {
        return """
        <!DOCTYPE html>
        <html lang="vi">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>{{title}}</title>
        </head>
        <body>
            <div class="hero">
                <h1>{{headline}}</h1>
                <p>{{subheadline}}</p>
            </div>
            <div class="booking-form">
                <h2>Đặt Lịch Hẹn</h2>
                <form id="leadForm">
                    <input type="text" name="fullName" placeholder="Họ và tên" required />
                    <input type="tel" name="phone" placeholder="Số điện thoại" required />
                    <input type="date" name="preferredDate" />
                    <select name="timeSlot">
                        <option value="">Chọn giờ</option>
                        <option value="09:00">09:00</option>
                        <option value="10:00">10:00</option>
                        <option value="14:00">14:00</option>
                        <option value="15:00">15:00</option>
                    </select>
                    <textarea name="message" placeholder="Ghi chú"></textarea>
                    <button type="submit" data-track="form-submit" data-element="booking-form">Đặt Lịch</button>
                </form>
            </div>
        </body>
        </html>
        """;
    }

    private static string GetProductTemplate()
    {
        return """
        <!DOCTYPE html>
        <html lang="vi">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>{{title}}</title>
        </head>
        <body>
            <div class="hero">
                <h1>{{headline}}</h1>
                <p>{{subheadline}}</p>
                <div class="price">{{price}}</div>
            </div>
            <div class="features">
                <h2>Tính Năng</h2>
                <ul>{{features_list}}</ul>
            </div>
            <div class="lead-form">
                <form id="leadForm">
                    <input type="text" name="fullName" placeholder="Họ và tên" required />
                    <input type="tel" name="phone" placeholder="Số điện thoại" required />
                    <button type="submit" data-track="form-submit" data-element="product-inquiry">Tư Vấn Ngay</button>
                </form>
            </div>
        </body>
        </html>
        """;
    }
}
