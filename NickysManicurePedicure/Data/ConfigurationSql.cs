namespace NickysManicurePedicure.Data;

public static class ConfigurationSql
{
    // Keep check constraint expressions ANSI-friendly to reduce provider-specific rewrites later.
    public const string TestimonialRatingRange = "Rating >= 1 AND Rating <= 5";
    public const string BusinessHoursClosedTimes = "IsClosed = 1 OR (OpenTime IS NOT NULL AND CloseTime IS NOT NULL)";
    public const string BusinessHoursTimeRange = "IsClosed = 1 OR OpenTime < CloseTime";
}
