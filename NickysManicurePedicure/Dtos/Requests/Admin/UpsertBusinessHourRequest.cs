namespace NickysManicurePedicure.Dtos.Requests.Admin;

public sealed class UpsertBusinessHourRequest
{
    public string DayOfWeek { get; init; } = string.Empty;
    public bool IsClosed { get; init; }
    public TimeOnly? OpenTime { get; init; }
    public TimeOnly? CloseTime { get; init; }
    public string? Notes { get; init; }
    public int DisplayOrder { get; init; }
}
