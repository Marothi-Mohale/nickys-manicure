namespace NickysManicurePedicure.Dtos.Responses;

public sealed class BusinessProfileResponse
{
    public required int Id { get; init; }
    public required string BusinessName { get; init; }
    public required string Tagline { get; init; }
    public string? Description { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Email { get; init; }
    public required string AddressLine1 { get; init; }
    public required string City { get; init; }
    public required string Region { get; init; }
    public required string PostalCode { get; init; }
    public string? InstagramHandle { get; init; }
    public required int YearsOfExperience { get; init; }
    public string? HeroHeadline { get; init; }
    public string? HeroSubheadline { get; init; }
}
