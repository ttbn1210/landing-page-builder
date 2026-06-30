namespace TrackQR.Web.Helpers;

public static class DateTimeHelper
{
    public static string ToRelativeTime(DateTime dateTime)
    {
        var timeSpan = DateTime.UtcNow - dateTime;

        if (timeSpan.TotalMinutes < 1) return "just now";
        if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes}m ago";
        if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours}h ago";
        if (timeSpan.TotalDays < 30) return $"{(int)timeSpan.TotalDays}d ago";

        return dateTime.ToString("dd/MM/yyyy");
    }

    public static string ToShortDate(DateTime? dateTime)
    {
        return dateTime?.ToString("dd/MM/yyyy") ?? "-";
    }
}
