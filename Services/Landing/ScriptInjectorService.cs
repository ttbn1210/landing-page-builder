namespace TrackQR.Web.Services.Landing;

public class ScriptInjectorService
{
    public string GetTrackerScript(string sessionKey, string baseUrl)
    {
        return $"""
        <script>
            window.__TRACKQR__ = {"{"}
                sessionKey: '{sessionKey}',
                apiBase: '{baseUrl}'
            {"}"};
        </script>
        <script src="{baseUrl}/js/tracker.js" defer></script>
        """;
    }
}
