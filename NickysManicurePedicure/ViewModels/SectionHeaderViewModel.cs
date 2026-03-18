namespace NickysManicurePedicure.ViewModels;

public class SectionHeaderViewModel
{
    public required string Eyebrow { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string AlignmentClass { get; init; } = "text-center";
}
