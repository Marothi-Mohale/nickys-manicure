using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Tests.Infrastructure;

public static class TestDataFactory
{
    public static ServiceCategory CreateCategory(
        string name = "Manicure",
        string slug = "manicure",
        int displayOrder = 1) => new()
        {
            Name = name,
            Slug = slug,
            Description = $"{name} services",
            Status = ContentStatus.Published,
            DisplayOrder = displayOrder
        };

    public static Service CreateService(
        int serviceCategoryId,
        string name = "Luxury Gel Manicure",
        string slug = "luxury-gel-manicure",
        ContentStatus status = ContentStatus.Published,
        bool isFeatured = true,
        int displayOrder = 1,
        decimal? priceFromAmount = 520m) => new()
        {
            ServiceCategoryId = serviceCategoryId,
            Name = name,
            Slug = slug,
            Description = "Premium nail service.",
            DurationLabel = "75 min",
            PriceFromLabel = "From R520",
            PriceFromAmount = priceFromAmount,
            IsFeatured = isFeatured,
            Status = status,
            DisplayOrder = displayOrder
        };

    public static CreateBookingRequestDto CreateBookingRequest(
        int? preferredServiceId = null,
        string? preferredServiceName = null) => new()
        {
            FullName = "  Nicky Client  ",
            Email = "  nicky@example.com  ",
            PhoneNumber = "  +27682518739  ",
            PreferredServiceId = preferredServiceId,
            PreferredServiceName = preferredServiceName,
            PreferredDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(2)),
            PreferredTime = new TimeOnly(10, 30),
            Message = "  I would love a weekday appointment if one is available.  ",
            SourcePage = "  Homepage  "
        };

    public static CreateContactInquiryDto CreateContactInquiryRequest() => new()
    {
        FullName = "  Client Name  ",
        Email = "  client@example.com  ",
        PhoneNumber = "  +27682518739  ",
        Subject = "  Booking question  ",
        Message = "  I would like to know which premium manicure lasts the longest.  ",
        SourcePage = "  ContactPage  "
    };

    public static BookingRequest CreateBookingEntity(
        string fullName,
        string email,
        string requestedServiceName,
        BookingRequestStatus status,
        DateOnly preferredDate,
        TimeOnly preferredTime,
        DateTime createdAtUtc) => new()
        {
            FullName = fullName,
            Email = email,
            PhoneNumber = "+27682518739",
            RequestedServiceName = requestedServiceName,
            Status = status,
            PreferredDate = preferredDate,
            PreferredTime = preferredTime,
            Message = "Please confirm availability.",
            SourcePage = "AdminImport",
            CreatedAtUtc = createdAtUtc,
            UpdatedAtUtc = createdAtUtc
        };

    public static ContactInquiry CreateContactInquiryEntity(
        string fullName,
        string email,
        string subject,
        ContactInquiryStatus status,
        DateTime createdAtUtc) => new()
        {
            FullName = fullName,
            Email = email,
            PhoneNumber = "+27682518739",
            Subject = subject,
            Message = "Please share your availability.",
            Status = status,
            SourcePage = "ContactPage",
            CreatedAtUtc = createdAtUtc,
            UpdatedAtUtc = createdAtUtc
        };
}
