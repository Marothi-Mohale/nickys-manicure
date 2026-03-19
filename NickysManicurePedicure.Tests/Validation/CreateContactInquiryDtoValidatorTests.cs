using FluentValidation.TestHelper;
using NickysManicurePedicure.Tests.Infrastructure;
using NickysManicurePedicure.Validation;
using Xunit;

namespace NickysManicurePedicure.Tests.Validation;

public sealed class CreateContactInquiryDtoValidatorTests
{
    [Fact]
    public async Task ValidateAsync_RejectsInvalidEmailAndShortMessage()
    {
        var validator = new CreateContactInquiryDtoValidator();
        var request = new NickysManicurePedicure.Dtos.Requests.CreateContactInquiryDto
        {
            FullName = "Client Name",
            Email = "not-an-email",
            PhoneNumber = "+27682518739",
            Subject = "Booking question",
            Message = "short",
            SourcePage = "ContactPage"
        };

        var result = await validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Message);
    }

    [Fact]
    public async Task ValidateAsync_AcceptsValidInquiry()
    {
        var validator = new CreateContactInquiryDtoValidator();
        var request = new NickysManicurePedicure.Dtos.Requests.CreateContactInquiryDto
        {
            FullName = "Client Name",
            Email = "client@example.com",
            PhoneNumber = "+27 68 251 8739",
            Subject = "Booking question",
            Message = "I would like to know which premium manicure lasts the longest.",
            SourcePage = "ContactPage"
        };

        var result = await validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
