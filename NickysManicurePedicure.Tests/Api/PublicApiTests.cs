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

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<SalonServiceResponse>>();

        Assert.NotNull(payload);
        Assert.Equal(1, payload.Page);
        Assert.Equal(2, payload.PageSize);
        Assert.NotEmpty(payload.Items);
        Assert.All(payload.Items, item => Assert.True(item.IsFeatured));
    }

    [Fact]
    public async Task GetBusinessProfile_ReturnsStructuredHours()
    {
        var response = await _client.GetAsync("/api/business/profile");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<BusinessProfileResponse>();

        Assert.NotNull(payload);
        Assert.Equal("Nicky's Manicure & Pedicure", payload.Name);
        Assert.NotEmpty(payload.BusinessHours);
        Assert.Contains(payload.BusinessHours, hour => hour.DayOfWeek == DayOfWeek.Monday.ToString());
    }

    [Fact]
    public async Task PostContactInquiry_ReturnsAccepted()
    {
        var request = new CreateContactInquiryDto
        {
            FullName = "Test Client",
            Email = "client@example.com",
            PhoneNumber = "+27682518739",
            Message = "I would like to ask about availability for next week.",
            SourcePage = "Api"
        };

        var response = await _client.PostAsJsonAsync("/api/contact-inquiries", request);

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ContactInquiryAcceptedResponse>();
        Assert.NotNull(payload);
        Assert.True(payload.InquiryId > 0);
    }

    [Fact]
    public async Task PostBookingRequest_WithInvalidPayload_ReturnsValidationProblemDetails()
    {
        var request = new CreateBookingRequestDto
        {
            FullName = "Test Client",
            Email = "not-an-email",
            PhoneNumber = "123",
            PreferredService = "",
            PreferredDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-1)),
            PreferredTime = new TimeOnly(10, 0),
            Message = "short"
        };

        var response = await _client.PostAsJsonAsync("/api/booking-requests", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(payload);
        Assert.Contains("Email", payload.Errors.Keys);
    }
}
