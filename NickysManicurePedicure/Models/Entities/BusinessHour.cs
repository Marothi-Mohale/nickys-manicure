namespace NickysManicurePedicure.Models.Entities;

public class BusinessHour
{
    public int Id { get; set; }
    public int BusinessProfileId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public bool IsClosed { get; set; }
    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }
    public string? Notes { get; set; }
    public int DisplayOrder { get; set; }

    public BusinessProfile? BusinessProfile { get; set; }
}
