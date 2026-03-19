using FluentValidation.TestHelper;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Validation;
using Xunit;

namespace NickysManicurePedicure.Tests.Validation;

public sealed class QueryAndStatusValidatorTests
{
    [Fact]
    public async Task BookingQueryValidator_RejectsOversizedPageAndInvalidDateRange()
    {
        var validator = new BookingQueryParametersValidator();
        var request = new BookingQueryParameters
        {
            Page = 1,
            PageSize = 101,
            FromDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(5)),
            ToDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(1)),
            SortBy = "createdAtUtc",
            SortDirection = "desc"
        };

        var result = await validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.PageSize);
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public async Task ContactInquiryQueryValidator_RejectsInvalidStatus()
    {
        var validator = new ContactInquiryQueryParametersValidator();

        var result = await validator.TestValidateAsync(new ContactInquiryQueryParameters
        {
            Status = "Unknown",
            SortBy = "createdAtUtc",
            SortDirection = "desc"
        });

        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Fact]
    public async Task UpdateBookingStatusValidator_RejectsUnknownStatus()
    {
        var validator = new UpdateBookingStatusDtoValidator();

        var result = await validator.TestValidateAsync(new UpdateBookingStatusDto
        {
            Status = "Unknown",
            AdminNotes = "Handled."
        });

        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Fact]
    public async Task UpdateContactInquiryStatusValidator_RejectsOverlongNotes()
    {
        var validator = new UpdateContactInquiryStatusDtoValidator();

        var result = await validator.TestValidateAsync(new UpdateContactInquiryStatusDto
        {
            Status = "Responded",
            AdminNotes = new string('a', 1001)
        });

        result.ShouldHaveValidationErrorFor(x => x.AdminNotes);
    }
}
