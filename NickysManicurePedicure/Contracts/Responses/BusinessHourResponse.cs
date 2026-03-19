namespace NickysManicurePedicure.Contracts.Responses;

public sealed class BusinessHourResponse
{
    public required int Id { get; init; }
    public required string DayOfWeek { get; init; }
    public required bool IsClosed { get; init; }
    public TimeOnly? OpenTime { get; init; }
    public TimeOnly? CloseTime { get; init; }
    public string? Notes { get; init; }
    public required int DisplayOrder { get; init; }
}
