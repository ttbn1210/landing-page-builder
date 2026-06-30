using TrackQR.Web.Models;

namespace TrackQR.Web.Services.Landing;

public class HtmlRenderService
{
    private readonly ScriptInjectorService _injector;

    public HtmlRenderService(ScriptInjectorService injector)
    {
        _injector = injector;
    }

    public string RenderPage(LandingPage page, string sessionKey, string baseUrl)
    {
        var html = page.HtmlContent ?? "<html><body><h1>Page Not Found</h1></body></html>";

        // Inject CSS
        if (!string.IsNullOrEmpty(page.CssContent))
        {
            var styleTag = $"<style>{page.CssContent}</style>";
            html = InjectBeforeClose(html, "</head>", styleTag);
        }

        // Inject head scripts
        if (!string.IsNullOrEmpty(page.HeadScripts))
        {
            html = InjectBeforeClose(html, "</head>", page.HeadScripts);
        }

        // Inject tracker.js with session context
        var trackerScript = _injector.GetTrackerScript(sessionKey, baseUrl);
        html = InjectBeforeClose(html, "</body>", trackerScript);

        // Inject body scripts
        if (!string.IsNullOrEmpty(page.BodyScripts))
        {
            html = InjectBeforeClose(html, "</body>", page.BodyScripts);
        }

        return html;
    }

    private static string InjectBeforeClose(string html, string closeTag, string content)
    {
        var index = html.LastIndexOf(closeTag, StringComparison.OrdinalIgnoreCase);
        if (index < 0)
            return html + content;
        return html.Insert(index, content);
    }
}
