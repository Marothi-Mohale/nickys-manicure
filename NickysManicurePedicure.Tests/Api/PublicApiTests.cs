using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;
using NickysManicurePedicure.Tests.Infrastructure;
using Xunit;

namespace NickysManicurePedicure.Tests.Api;

public sealed class PublicApiTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public PublicApiTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetServices_ReturnsPagedCatalog()
    {
        var response = await _client.GetAsync("/api/services?page=1&pageSize=2&featuredOnly=true");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<ServiceListItemResponse>>();

        Assert.NotNull(payload);
        Assert.Equal(1, payload.Pagination.Page);
        Assert.Equal(2, payload.Pagination.PageSize);
        Assert.InRange(payload.Pagination.CurrentItemCount, 1, 2);
        Assert.True(payload.Pagination.TotalCount >= payload.Pagination.CurrentItemCount);
        Assert.NotEmpty(payload.Items);
        Assert.All(payload.Items, item => Assert.True(item.IsFeatured));
    }

    [Fact]
    public async Task GetServices_CanFilterByCategory()
    {
        var response = await _client.GetAsync("/api/services?category=pedicure");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<ServiceListItemResponse>>();

        Assert.NotNull(payload);
        Assert.NotEmpty(payload.Items);
        Assert.All(payload.Items, item => Assert.Equal("pedicure", item.Category.Slug));
    }

    [Fact]
    public async Task GetServices_WithOversizedPageSize_ReturnsValidationProblemDetails()
    {
        var response = await _client.GetAsync("/api/services?page=1&pageSize=1000");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(payload);
        Assert.True(payload.Errors.ContainsKey("PageSize"));
    }

    [Fact]
    public async Task GetServices_CanSortByPriceAscending()
    {
        var response = await _client.GetAsync("/api/services?sortBy=price&sortDirection=asc");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<ServiceListItemResponse>>();

        Assert.NotNull(payload);
        var pricedItems = payload.Items
            .Where(item => item.PriceFromAmount.HasValue)
            .ToList();

        Assert.NotEmpty(pricedItems);
        Assert.Equal(pricedItems.OrderBy(item => item.PriceFromAmount).Select(item => item.Id), pricedItems.Select(item => item.Id));
    }

    [Fact]
    public async Task GetServiceById_ReturnsServiceDetail()
    {
        var listResponse = await _client.GetAsync("/api/services?page=1&pageSize=1");
        listResponse.EnsureSuccessStatusCode();

        var listPayload = await listResponse.Content.ReadFromJsonAsync<PagedResponse<ServiceListItemResponse>>();
        Assert.NotNull(listPayload);

        var serviceId = listPayload.Items.First().Id;
        var detailResponse = await _client.GetAsync($"/api/services/{serviceId}");

        detailResponse.EnsureSuccessStatusCode();

        var detailPayload = await detailResponse.Content.ReadFromJsonAsync<ServiceDetailResponse>();
        Assert.NotNull(detailPayload);
        Assert.Equal(serviceId, detailPayload.Id);
    }

    [Fact]
    public async Task GetServiceCategories_ReturnsPublishedCategories()
    {
        var response = await _client.GetAsync("/api/service-categories");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<ServiceCategoryListItemResponse>>();

        Assert.NotNull(payload);
        Assert.NotEmpty(payload);
        Assert.All(payload, item => Assert.True(item.ServiceCount >= 0));
    }

    [Fact]
    public async Task GetBusinessProfile_ReturnsFrontendFields()
    {
        var response = await _client.GetAsync("/api/business-profile");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<BusinessProfileResponse>();

        Assert.NotNull(payload);
        Assert.Equal("Nicky's Manicure & Pedicure", payload.BusinessName);
        Assert.Equal("Cape Town", payload.City);
        Assert.True(payload.YearsOfExperience > 0);
        Assert.False(string.IsNullOrWhiteSpace(payload.HeroHeadline));
        Assert.False(string.IsNullOrWhiteSpace(payload.HeroSubheadline));
    }

    [Fact]
    public async Task GetBusinessHours_ReturnsStructuredHours()
    {
        var response = await _client.GetAsync("/api/business-hours");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<BusinessHourResponse>>();

        Assert.NotNull(payload);
        Assert.NotEmpty(payload);
        Assert.Contains(payload, hour => hour.DayOfWeek == DayOfWeek.Monday.ToString());
    }

    [Fact]
    public async Task GetMissingBooking_ReturnsProblemDetailsNotFound()
    {
        var response = await _client.GetAsync("/api/bookings/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(payload);
        Assert.Equal(404, payload.Status);
        Assert.Equal("resource_not_found", payload.Extensions["errorCode"]?.ToString());
        Assert.NotNull(payload.Extensions["traceId"]);
        Assert.NotNull(payload.Extensions["correlationId"]);
    }

    [Fact]
    public async Task GetUnknownApiRoute_ReturnsProblemDetailsNotFound()
    {
        var response = await _client.GetAsync("/api/does-not-exist");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(payload);
        Assert.Equal(404, payload.Status);
        Assert.Equal("resource_not_found", payload.Extensions["errorCode"]?.ToString());
    }

    [Fact]
    public async Task GetUnknownApiRoute_EchoesCorrelationId()
    {
        const string correlationId = "test-correlation-id";
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/does-not-exist");
        request.Headers.Add("X-Correlation-ID", correlationId);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.True(response.Headers.TryGetValues("X-Correlation-ID", out var headerValues));
        Assert.Contains(correlationId, headerValues);

        var payload = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(payload);
        Assert.Equal(correlationId, payload.Extensions["correlationId"]?.ToString());
    }

    [Fact]
    public async Task GetTestimonials_ReturnsApprovedTestimonialsInDisplayOrder()
    {
        var response = await _client.GetAsync("/api/testimonials?page=1&pageSize=10&sortBy=displayOrder&sortDirection=asc");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<TestimonialListItemResponse>>();

        Assert.NotNull(payload);
        Assert.NotEmpty(payload.Items);
        Assert.Equal(payload.Items.OrderBy(item => item.DisplayOrder).Select(item => item.Id), payload.Items.Select(item => item.Id));
        Assert.All(payload.Items, item =>
        {
            Assert.False(string.IsNullOrWhiteSpace(item.ClientName));
            Assert.False(string.IsNullOrWhiteSpace(item.Quote));
            Assert.InRange(item.Rating, 1, 5);
        });
    }

    [Fact]
    public async Task GetGallery_ReturnsMetadataAndSupportsCategoryFiltering()
    {
        var response = await _client.GetAsync("/api/gallery?page=1&pageSize=10&category=manicure&featuredOnly=true&sortBy=displayOrder&sortDirection=asc");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<GalleryListItemResponse>>();

        Assert.NotNull(payload);
        Assert.NotEmpty(payload.Items);
        Assert.Equal(payload.Items.OrderBy(item => item.DisplayOrder).Select(item => item.Id), payload.Items.Select(item => item.Id));
        Assert.All(payload.Items, item =>
        {
            Assert.Equal("Manicure", item.Category);
            Assert.True(item.IsFeatured);
            Assert.False(string.IsNullOrWhiteSpace(item.Title));
            Assert.False(string.IsNullOrWhiteSpace(item.ImageUrl));
            Assert.False(string.IsNullOrWhiteSpace(item.AltText));
        });
    }

    [Fact]
    public async Task GetFaqs_ReturnsActiveItemsInDisplayOrder()
    {
        var response = await _client.GetAsync("/api/faqs?page=1&pageSize=10&sortBy=displayOrder&sortDirection=asc");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<FaqListItemResponse>>();

        Assert.NotNull(payload);
        Assert.NotEmpty(payload.Items);
        Assert.Equal(payload.Items.OrderBy(item => item.DisplayOrder).Select(item => item.Id), payload.Items.Select(item => item.Id));
        Assert.All(payload.Items, item =>
        {
            Assert.False(string.IsNullOrWhiteSpace(item.Question));
            Assert.False(string.IsNullOrWhiteSpace(item.Answer));
        });
    }

    [Fact]
    public async Task PostContactInquiry_ReturnsAccepted()
    {
        var request = new CreateContactInquiryDto
        {
            FullName = "Test Client",
            Email = "client@example.com",
            PhoneNumber = "+27682518739",
            Subject = "Availability question",
            Message = "I would like to ask about availability for next week.",
            SourcePage = "Api"
        };

        var response = await _client.PostAsJsonAsync("/api/contact-inquiries", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ContactInquiryCreateResponse>();
        Assert.NotNull(payload);
        Assert.True(payload.InquiryId > 0);
        Assert.Equal($"/api/contact-inquiries/{payload.InquiryId}", payload.DetailUrl);
    }

    [Fact]
    public async Task GetContactInquiries_ReturnsPagedInquiries()
    {
        var createRequest = new CreateContactInquiryDto
        {
            FullName = "List Inquiry Client",
            Email = "list-inquiry@example.com",
            PhoneNumber = "+27682518739",
            Subject = "General question",
            Message = "I would like to ask a few questions about your services.",
            SourcePage = "Api"
        };

        await _client.PostAsJsonAsync("/api/contact-inquiries", createRequest);

        var response = await _client.GetAsync("/api/contact-inquiries?page=1&pageSize=10");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<ContactInquiryReadResponse>>();
        Assert.NotNull(payload);
        Assert.NotEmpty(payload.Items);
    }

    [Fact]
    public async Task GetContactInquiryById_ReturnsInquiry()
    {
        var createRequest = new CreateContactInquiryDto
        {
            FullName = "Detail Inquiry Client",
            Email = "detail-inquiry@example.com",
            PhoneNumber = "+27682518739",
            Subject = "Follow-up question",
            Message = "I want to know more about booking options.",
            SourcePage = "Api"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/contact-inquiries", createRequest);
        var createPayload = await createResponse.Content.ReadFromJsonAsync<ContactInquiryCreateResponse>();
        Assert.NotNull(createPayload);

        var response = await _client.GetAsync($"/api/contact-inquiries/{createPayload.InquiryId}");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<ContactInquiryReadResponse>();
        Assert.NotNull(payload);
        Assert.Equal(createPayload.InquiryId, payload.Id);
    }

    [Fact]
    public async Task PatchContactInquiryStatus_UpdatesInquiry()
    {
        var createRequest = new CreateContactInquiryDto
        {
            FullName = "Patch Inquiry Client",
            Email = "patch-inquiry@example.com",
            PhoneNumber = "+27682518739",
            Subject = "Status check",
            Message = "Please let me know whether you received my message.",
            SourcePage = "Api"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/contact-inquiries", createRequest);
        var createPayload = await createResponse.Content.ReadFromJsonAsync<ContactInquiryCreateResponse>();
        Assert.NotNull(createPayload);

        var patchResponse = await _client.PatchAsJsonAsync(
            $"/api/contact-inquiries/{createPayload.InquiryId}/status",
            new UpdateContactInquiryStatusDto
            {
                Status = "Responded",
                AdminNotes = "Responded by admin."
            });

        patchResponse.EnsureSuccessStatusCode();

        var patchPayload = await patchResponse.Content.ReadFromJsonAsync<ContactInquiryReadResponse>();
        Assert.NotNull(patchPayload);
        Assert.Equal("Responded", patchPayload.Status);
    }

    [Fact]
    public async Task PostBookingRequest_WithInvalidPayload_ReturnsValidationProblemDetails()
    {
        var request = new CreateBookingRequestDto
        {
            FullName = "Test Client",
            Email = "not-an-email",
            PhoneNumber = "123",
            PreferredServiceName = "",
            PreferredDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-1)),
            PreferredTime = new TimeOnly(10, 0),
            Message = "short"
        };

        var response = await _client.PostAsJsonAsync("/api/bookings", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(payload);
        Assert.Contains("Email", payload.Errors.Keys);
    }

    [Fact]
    public async Task PostBooking_CreatesAndReturnsCreatedResponse()
    {
        var request = new CreateBookingRequestDto
        {
            FullName = "Test Client",
            Email = "client@example.com",
            PhoneNumber = "+27682518739",
            PreferredServiceId = 1,
            PreferredDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(2)),
            PreferredTime = new TimeOnly(10, 30),
            Message = "I would like a weekday morning appointment if available.",
            SourcePage = "Api"
        };

        var response = await _client.PostAsJsonAsync("/api/bookings", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<BookingCreateResponse>();
        Assert.NotNull(payload);
        Assert.True(payload.BookingId > 0);
        Assert.Equal("Pending", payload.Status);
        Assert.Equal($"/api/bookings/{payload.BookingId}", payload.DetailUrl);
    }

    [Fact]
    public async Task GetBookings_ReturnsPagedBookings()
    {
        var createRequest = new CreateBookingRequestDto
        {
            FullName = "Admin Read Client",
            Email = "adminread@example.com",
            PhoneNumber = "+27682518739",
            PreferredServiceId = 1,
            PreferredDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(3)),
            PreferredTime = new TimeOnly(11, 0),
            Message = "Please book me for a manicure.",
            SourcePage = "Api"
        };

        await _client.PostAsJsonAsync("/api/bookings", createRequest);

        var response = await _client.GetAsync("/api/bookings?page=1&pageSize=10");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<BookingReadResponse>>();
        Assert.NotNull(payload);
        Assert.NotEmpty(payload.Items);
    }

    [Fact]
    public async Task GetAdminBookingsRoute_CanFilterByStatus()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/bookings", new CreateBookingRequestDto
        {
            FullName = "Admin Filter Client",
            Email = "admin-filter@example.com",
            PhoneNumber = "+27682518739",
            PreferredServiceId = 1,
            PreferredDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(6)),
            PreferredTime = new TimeOnly(15, 0),
            Message = "Please confirm my luxury service.",
            SourcePage = "Api"
        });

        var createdBooking = await createResponse.Content.ReadFromJsonAsync<BookingCreateResponse>();
        Assert.NotNull(createdBooking);

        var patchResponse = await _client.PatchAsJsonAsync(
            $"/api/admin/bookings/{createdBooking.BookingId}/status",
            new UpdateBookingStatusDto
            {
                Status = "Confirmed",
                AdminNotes = "Confirmed for admin route test."
            });

        patchResponse.EnsureSuccessStatusCode();

        var response = await _client.GetAsync("/api/admin/bookings?status=Confirmed&page=1&pageSize=10");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<BookingReadResponse>>();
        Assert.NotNull(payload);
        Assert.Contains(payload.Items, item => item.Id == createdBooking.BookingId && item.Status == "Confirmed");
    }

    [Fact]
    public async Task GetAdminContactInquiriesRoute_CanFilterByStatus()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/contact-inquiries", new CreateContactInquiryDto
        {
            FullName = "Admin Inquiry Filter",
            Email = "admin-inquiry@example.com",
            PhoneNumber = "+27682518739",
            Subject = "Status update",
            Message = "Please let me know if there is a bridal package available.",
            SourcePage = "Api"
        });

        var createdInquiry = await createResponse.Content.ReadFromJsonAsync<ContactInquiryCreateResponse>();
        Assert.NotNull(createdInquiry);

        var patchResponse = await _client.PatchAsJsonAsync(
            $"/api/admin/contact-inquiries/{createdInquiry.InquiryId}/status",
            new UpdateContactInquiryStatusDto
            {
                Status = "Responded",
                AdminNotes = "Responded for admin route test."
            });

        patchResponse.EnsureSuccessStatusCode();

        var response = await _client.GetAsync("/api/admin/contact-inquiries?status=Responded&page=1&pageSize=10");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<ContactInquiryReadResponse>>();
        Assert.NotNull(payload);
        Assert.Contains(payload.Items, item => item.Id == createdInquiry.InquiryId && item.Status == "Responded");
    }

    [Fact]
    public async Task PatchBookingStatus_UpdatesBooking()
    {
        var createRequest = new CreateBookingRequestDto
        {
            FullName = "Status Client",
            Email = "status@example.com",
            PhoneNumber = "+27682518739",
            PreferredServiceId = 2,
            PreferredDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(4)),
            PreferredTime = new TimeOnly(12, 0),
            Message = "Please confirm my pedicure request.",
            SourcePage = "Api"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/bookings", createRequest);
        var createPayload = await createResponse.Content.ReadFromJsonAsync<BookingCreateResponse>();
        Assert.NotNull(createPayload);

        var patchResponse = await _client.PatchAsJsonAsync(
            $"/api/bookings/{createPayload.BookingId}/status",
            new UpdateBookingStatusDto
            {
                Status = "Confirmed",
                AdminNotes = "Confirmed by admin for Friday."
            });

        patchResponse.EnsureSuccessStatusCode();

        var patchPayload = await patchResponse.Content.ReadFromJsonAsync<BookingReadResponse>();
        Assert.NotNull(patchPayload);
        Assert.Equal("Confirmed", patchPayload.Status);
    }
}
