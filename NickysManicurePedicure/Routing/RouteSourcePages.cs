namespace NickysManicurePedicure.Routing;

public static class RouteSourcePages
{
    public const string Home = "Home";
    public const string Services = "Services";
    public const string Booking = "Booking";
    public const string Contact = "Contact";

    public static bool IsKnown(string? sourcePage)
    {
        if (string.IsNullOrWhiteSpace(sourcePage))
        {
            return false;
        }

        return sourcePage.Trim() switch
        {
            Home => true,
            Services => true,
            Booking => true,
            Contact => true,
            _ => false
        };
    }

    public static string Normalize(string? sourcePage, string fallback) =>
        IsKnown(sourcePage) ? sourcePage!.Trim() : fallback;
}
