namespace TrackQR.Web.Services.Tracking;

public class GeoLocationService
{
    public GeoInfo Resolve(string? ipAddress)
    {
        // In production, integrate with MaxMind GeoIP or ip-api.com
        // For MVP, return placeholder data
        return new GeoInfo
        {
            Country = "Vietnam",
            Province = "Ho Chi Minh",
            City = "Ho Chi Minh City"
        };
    }
}

public class GeoInfo
{
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? City { get; set; }
}
