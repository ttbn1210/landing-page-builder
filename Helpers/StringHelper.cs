using System.Text.RegularExpressions;

namespace TrackQR.Web.Helpers;

public static partial class StringHelper
{
    public static string ToSlug(string input)
    {
        var slug = input.ToLowerInvariant().Trim();
        slug = SlugRegex().Replace(slug, "");
        slug = SpaceRegex().Replace(slug, "-");
        slug = DashRegex().Replace(slug, "-");
        return slug.Trim('-');
    }

    public static string Truncate(string? input, int maxLength)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        return input.Length <= maxLength ? input : input[..maxLength] + "...";
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex SlugRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex SpaceRegex();

    [GeneratedRegex(@"-+")]
    private static partial Regex DashRegex();
}
