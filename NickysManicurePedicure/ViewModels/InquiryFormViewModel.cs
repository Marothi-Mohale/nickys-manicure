using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Routing;

namespace NickysManicurePedicure.ViewModels;

public class InquiryFormViewModel : IValidatableObject
{
    public InquiryType InquiryType { get; set; } = InquiryType.Booking;

    [Display(Name = "Full name")]
    [Required(ErrorMessage = "Please enter your full name so we know who to contact.")]
    [StringLength(120, ErrorMessage = "Your full name must be 120 characters or fewer.")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Email address")]
    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(200, ErrorMessage = "Your email address must be 200 characters or fewer.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Phone number")]
    [Required(ErrorMessage = "Please enter your phone number so we can reply quickly.")]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [StringLength(30, ErrorMessage = "Your phone number must be 30 characters or fewer.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please tell us how we can help.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Please enter between 10 and 2000 characters.")]
    public string Message { get; set; } = string.Empty;

    [StringLength(30, ErrorMessage = "The source page value is too long.")]
    public string SourcePage { get; set; } = "Home";

    public string SubmitLabel => "Send Message";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(FullName))
        {
            yield return new ValidationResult(
                "Please enter your full name so we know who to contact.",
                [nameof(FullName)]);
        }

        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            yield return new ValidationResult(
                "Please enter your phone number so we can reply quickly.",
                [nameof(PhoneNumber)]);
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            yield return new ValidationResult(
                "Please enter your email address.",
                [nameof(Email)]);
        }

        if (!string.IsNullOrWhiteSpace(Message) && Message.Trim().Length < 10)
        {
            yield return new ValidationResult(
                "Please tell us a little more so we can assist properly.",
                [nameof(Message)]);
        }

        if (!RouteSourcePages.IsKnown(SourcePage))
        {
            yield return new ValidationResult(
                "The message source was not recognized.",
                [nameof(SourcePage)]);
        }
    }
}
