namespace NickysManicurePedicure.Models.Entities;

public class Testimonial
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string Highlight { get; set; } = string.Empty;
    public string Review { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
