namespace TrackQR.Web.Infrastructure.Cache;

public static class CacheKeys
{
    public static string QRCode(string shortCode) => $"qr:{shortCode}";
    public static string LandingPage(Guid id) => $"lp:{id}";
    public static string DashboardStats => "dashboard:stats";
    public static string CampaignList => "campaigns:list";
    public static string Campaign(Guid id) => $"campaign:{id}";
}
