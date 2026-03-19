using FluentValidation;
using NickysManicurePedicure.Dtos.Requests;

namespace NickysManicurePedicure.Validation;

public sealed class CreateContactInquiryDtoValidator : AbstractValidator<CreateContactInquiryDto>
{
    public CreateContactInquiryDtoValidator()
    {
        RuleFor(x => x.FullName)
            .TrimmedRequiredText(120, "Full name");

        RuleFor(x => x.Email)
            .TrimmedRequiredText(200, "Email")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.PhoneNumber)
            .SalonPhone()
            .MaximumLength(30).WithMessage("Phone number must be 30 characters or fewer.");

        RuleFor(x => x.Subject)
            .MaximumLength(160).WithMessage("Subject must be 160 characters or fewer.");

        RuleFor(x => x.Message)
            .TrimmedRequiredText(2000, "Message")
            .MinimumLength(10).WithMessage("Message must be at least 10 characters long.");

        RuleFor(x => x.SourcePage)
            .OptionalMaxLength(30, "Source page");
    }
}
