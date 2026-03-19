namespace NickysManicurePedicure.Dtos.Requests.Admin;

public sealed class UpsertBusinessProfileRequest
{
    public string BusinessName { get; init; } = string.Empty;
    public string Tagline { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string AddressLine1 { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;
    public string? InstagramHandle { get; init; }
    public int YearsOfExperience { get; init; }
    public string? HeroHeadline { get; init; }
    public string? HeroSubheadline { get; init; }
    public IReadOnlyCollection<UpsertBusinessHourRequest> BusinessHours { get; init; } = [];
}
