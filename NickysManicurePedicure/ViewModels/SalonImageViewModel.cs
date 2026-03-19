namespace NickysManicurePedicure.ViewModels;

public class SalonImageViewModel
{
    public required string ImageUrl { get; init; }
    public required string AltText { get; init; }
    public string? Caption { get; init; }
}
