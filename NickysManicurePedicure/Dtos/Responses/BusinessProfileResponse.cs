namespace NickysManicurePedicure.Dtos.Responses;

public sealed class BusinessProfileResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Tagline { get; init; }
    public required string Phone { get; init; }
    public required string PhoneHref { get; init; }
    public required string Email { get; init; }
    public required string AddressLine1 { get; init; }
    public required string Suburb { get; init; }
    public required string City { get; init; }
    public required string Region { get; init; }
    public required string PostalCode { get; init; }
    public required string WhatsAppHref { get; init; }
    public string? InstagramHandle { get; init; }
    public string? BookingPolicy { get; init; }
    public string? AboutSummary { get; init; }
    public required IReadOnlyCollection<BusinessHourResponse> BusinessHours { get; init; }
}
